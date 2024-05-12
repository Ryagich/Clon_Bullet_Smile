using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.Serialization;

public class EnemyShooter : MonoBehaviour
{
    public UnityEvent Shoot;
    public UnityEvent StopShoot;

    [SerializeField] private TurretSettings _settings;

    private bool shooting;
    private bool characterVisibility;
    private Coroutine shootingCor;

    private void Start()
    {
        StartCoroutine(Checking());
    }

    public void ChangeVisibility(bool visibility)
    {
        characterVisibility = visibility;
        if (!visibility)
        {
            StopShooting();
        }
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            if (!Target.Instance.Health.Alive)
            {
                StopShooting();
                break;
            }
            var targetInRadius = CheckFieldOfView();
            if (targetInRadius && characterVisibility)
            {
                if (shootingCor == null)
                {
                    shootingCor = StartCoroutine(Shooting());
                }
            }
            else
            {
                StopShooting();
            }

            yield return new WaitForSeconds(_settings.ShootCheckTime);
        }
    }

    private void StopShooting()
    {
        if (shooting)
        {
            shooting = false;
            StopCoroutine(shootingCor);
            StopShoot?.Invoke();
            shootingCor = null;
        }
    }

    private IEnumerator Shooting()
    {
        shooting = true;
        while (true)
        {
            Shoot?.Invoke();
            yield return new WaitForSeconds(_settings.ShootTime);
        }
    }

    private bool CheckFieldOfView()
    {
        var targetPos = Target.Instance.transform.position;
        var pos = transform.position;
        var distance = Vector3.Distance(targetPos, pos);
        if (distance > _settings.Radius)
        {
            return false;
        }

        var dirToTarget = targetPos - pos;
        var inFOVCondition = (Vector3.Angle(transform.right, dirToTarget) < _settings.ShootAngle / 2);
        if (!inFOVCondition)
        {
            return false;
        }

        return true;
    }
}