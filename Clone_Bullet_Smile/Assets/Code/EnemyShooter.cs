using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShooter : MonoBehaviour
{
    public UnityEvent Shoot;
    public UnityEvent StopShoot;

    [SerializeField] private TurretSettings _settings;

    private bool characterVisibility;
    private Coroutine shootingCoroutine;

    private void Start()
    {
        StartCoroutine(UpdateShootStateCoroutine());
    }

    public void ChangeCharacterVisibility(bool visibility)
    {
        characterVisibility = visibility;
        if (!visibility)
        {
            StopShootCoroutine();
        }
    }

    private IEnumerator UpdateShootStateCoroutine()
    {
        while (true)
        {
            if (!Target.Instance.Health.Alive)
            {
                StopShootCoroutine();
                break;
            }
            var targetInFov = FOVUtils.IsInFOV(transform, Target.Instance.transform.position,
                _settings.Radius, _settings.ShootAngle);
            if (targetInFov && characterVisibility)
            {
                shootingCoroutine ??= StartCoroutine(ShootCoroutine());
            }
            else
            {
                StopShootCoroutine();
            }

            yield return new WaitForSeconds(_settings.ShootCheckTime);
        }
    }

    private void StopShootCoroutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            StopShoot?.Invoke();
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot?.Invoke();
            yield return new WaitForSeconds(_settings.ShootTime);
        }
    }
}