using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserShooter : MonoBehaviour
{
    public UnityEvent EndLaser;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField, Min(0)] private int _damage = 1;
    [SerializeField] private Transform _parent;
    [SerializeField] private EnemyFoV _fov;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _laserTime = .1f;
    
    public void Shoot()
    {
        var parentPos = _parent.position;
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, parentPos);

        if (Physics.Raycast(parentPos, _parent.transform.right, out var hit, _fov.Radius, _layerMask))
        {
            Debug.DrawRay(parentPos, _parent.transform.right, Color.red, 4);
            _lineRenderer.SetPosition(1, hit.point);

            var health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.ChangeAmount(-_damage);
            }
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + _parent.transform.right.normalized * _fov.Radius);
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