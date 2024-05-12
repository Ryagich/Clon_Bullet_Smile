using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFoV : MonoBehaviour
{
    public UnityEvent TargetIn;
    public UnityEvent TargetOut;
    
    [SerializeField] private TurretSettings _settings;
    [SerializeField] private LineRenderer _lineRenderer;

    private bool targetInFOV;

    private void Start()
    {
        StartCoroutine(Checking());
    }

    private IEnumerator Checking()
    {
        while (true)
        {
            var isInFovNow = IsInFOV();
            
            switch (isInFovNow)
            {
                case true when !targetInFOV:
                    targetInFOV = true;
                    TargetIn?.Invoke();
                    break;
                case false when targetInFOV:
                    targetInFOV = false;
                    TargetOut?.Invoke();
                    break;
            }

            yield return new WaitForSeconds(_settings.FOVCheckTime);
        }
    }

    private bool IsInFOV()
    {
        var targetPos = Target.Instance.transform.position;
        var pos = transform.position;
        var distance = Vector3.Distance(targetPos, pos);
        if (distance > _settings.Radius)
        {
            return false;
        }

        var dirToTarget = targetPos - pos;
        var inFOVCondition = (Vector3.Angle(transform.right, dirToTarget) < _settings.FieldAngle / 2);
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
        var angleInRad = Mathf.Deg2Rad * _settings.FieldAngle;

        _lineRenderer.positionCount = _settings.FOVLineCount + 1 + 2;
        _lineRenderer.SetPosition(0, pos);
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pos);

        var startRad = rot.eulerAngles.z * Mathf.Deg2Rad;
        var offset = _settings.FieldAngle / 2 * Mathf.Deg2Rad * -1;
        for (var i = 0; i < _settings.FOVLineCount + 1; i++)
        {
            var delta = startRad + offset + (angleInRad / _settings.FOVLineCount * i);

            var x = Mathf.Cos(delta) * _settings.Radius;
            var y = Mathf.Sin(delta) * _settings.Radius;

            var curPos = new Vector3(pos.x + x, pos.y + y, 0);
            _lineRenderer.SetPosition(i + 1, curPos);
        }
    }

    private void OnDrawGizmos()
    {
        Draw();
    }
}