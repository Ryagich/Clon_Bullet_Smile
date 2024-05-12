using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LaserShooter : MonoBehaviour
{
    public UnityEvent EndLaser;
    
    [SerializeField] private TurretSettings _settings;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _parent;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _laserTime = .1f;

    public void Shoot()
    {
        var parentPos = _parent.position;
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, parentPos);

        if (Physics.Raycast(parentPos, _parent.transform.right, out var hit, _settings.Radius, _layerMask))
        {
            _lineRenderer.SetPosition(1, hit.point);

            if (hit.transform.TryGetComponent(out Health health))
            {
                health.ChangeAmount(-_settings.Damage);
            }
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + _parent.transform.right.normalized * _settings.Radius);
        }

        StartCoroutine(PutOutLaser());
    }

    private IEnumerator PutOutLaser()
    {
        yield return new WaitForSeconds(_laserTime);
        _lineRenderer.enabled = false;
        EndLaser?.Invoke();
    }
}