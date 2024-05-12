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

    [SerializeField] private EnemyFoV _fov;
    [SerializeField, Range(.0f, 360f)] private float _angle;
    [SerializeField] private float _shootTime;
    [SerializeField] private float _checkTime;

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

            yield return new WaitForSeconds(_checkTime);
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
            yield return new WaitForSeconds(_shootTime);
        }
    }

    private bool CheckFieldOfView()
    {
        var targetPos = _fov.Target.position;
        var pos = transform.position;
        var distance = Vector3.Distance(targetPos, pos);
        if (distance > _fov.Radius)
        {
            return false;
        }

        var dirToTarget = targetPos - pos;
        var inFOVCondition = (Vector3.Angle(transform.right, dirToTarget) < _angle / 2);
        if (!inFOVCondition)
        {
            return false;
        }

        return true;
    }

    // private void OnDrawGizmos()
    // {
    //     Handles.color = Color.white;
    //     var pos = transform.position;
    //     var viewAngleA = new Vector3(Mathf.Sin(-_angle / 2 * Mathf.Deg2Rad),  Mathf.Cos(-_angle / 2 * Mathf.Deg2Rad),0);
    //     var viewAngleB = new Vector3(Mathf.Sin(_angle / 2 * Mathf.Deg2Rad), Mathf.Cos(_angle / 2 * Mathf.Deg2Rad),0 );
    //     
    //     Handles.DrawWireArc(pos, -Vector3.forward, viewAngleA, _angle, _fov.Radius);
    //     Handles.DrawLine(pos, pos + viewAngleA * _fov.Radius);
    //     Handles.DrawLine(pos, pos + viewAngleB * _fov.Radius);
    // }
}