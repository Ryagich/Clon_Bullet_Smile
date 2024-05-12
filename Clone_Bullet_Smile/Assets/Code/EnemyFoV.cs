using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFoV : MonoBehaviour
{
    public UnityEvent TargetIn;
    public UnityEvent TargetOut;

    [field: SerializeField] public Transform Target{ get; private set; }
    [field: SerializeField] public Transform ShootPoint{ get; private set; }
    [field: SerializeField, Range(.0f, 50f)]
    public float Radius { get; private set; }
    
    [SerializeField, Range(.0f, 360f)] private float _angle;
    [SerializeField] private float _checkTime;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField, Min(1)] private int _lineCount = 100;

    private bool targetInRadius;

    private void Start()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            var checkTargetInRadius = CheckFieldOfView();
            if (checkTargetInRadius)
            {
                targetInRadius = true;
                TargetIn?.Invoke();
            }
            else if (targetInRadius)
            {
                targetInRadius = false;
                TargetOut?.Invoke();
            }

            yield return new WaitForSeconds(_checkTime);
        }
    }

    private bool CheckFieldOfView()
    {
        var targetPos = Target.position;
        var pos = transform.position;
        var distance = Vector3.Distance(targetPos, pos);
        if (distance > Radius)
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

    public void Draw()
    {
        var pos = transform.position;
        var rot = transform.rotation;
        var angleInRad = Mathf.Deg2Rad * _angle;

        _lineRenderer.positionCount = _lineCount + 1 + 2;
        _lineRenderer.SetPosition(0, pos);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pos);

        var startRad = rot.eulerAngles.z * Mathf.Deg2Rad;
        var offset = _angle / 2 * Mathf.Deg2Rad * -1;
        for (var i = 0; i < _lineCount + 1; i++)
        {
            var delta = startRad + offset + (angleInRad / _lineCount * i);

            var x = Mathf.Cos(delta) * Radius;
            var y = Mathf.Sin(delta) * Radius;

            var curPos = new Vector3(pos.x + x, pos.y + y, 0);
            _lineRenderer.SetPosition(i + 1, curPos);
        }
    }
    
    private void OnDrawGizmos()
    {
        Draw();
    }
}