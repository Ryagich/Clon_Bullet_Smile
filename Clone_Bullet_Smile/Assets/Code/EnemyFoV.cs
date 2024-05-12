using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFoV : MonoBehaviour
{
    public UnityEvent TargetEnter;
    public UnityEvent TargetStay;
    public UnityEvent TargetExit;

    [SerializeField] private TurretSettings _settings;
    [SerializeField] private LineRenderer _lineRenderer;

    private bool targetInFOV;
    private Vector3[] linePositions;

    private void Awake()
    {
        linePositions = new Vector3[_settings.FOVLineCount + 2]; // center and edge
        _lineRenderer.positionCount = linePositions.Length;
    }

    private void Start()
    {
        StartCoroutine(UpdateFovCoroutine());
    }

    private IEnumerator UpdateFovCoroutine()
    {
        while (true)
        {
            var isInFovNow = FOVUtils.IsInFOV(transform, Target.Instance.transform.position,
                _settings.Radius, _settings.FieldAngle);

            switch (isInFovNow)
            {
                case true when !targetInFOV:
                    targetInFOV = true;
                    TargetEnter?.Invoke();
                    break;
                case false when targetInFOV:
                    targetInFOV = false;
                    TargetExit?.Invoke();
                    break;
                case true when targetInFOV:
                    TargetStay?.Invoke();
                    break;
            }

            yield return new WaitForSeconds(_settings.FOVCheckTime);
        }
    }

    private void UpdateLinePositions()
    {
        var pos = transform.position;
        var rot = transform.rotation;
        var angleInRad = Mathf.Deg2Rad * _settings.FieldAngle;

        linePositions[0] = pos;

        var startRad = rot.eulerAngles.z * Mathf.Deg2Rad;
        var offset = _settings.FieldAngle / 2 * Mathf.Deg2Rad * -1;
        for (var i = 0; i < _settings.FOVLineCount + 1; i++)
        {
            var delta = startRad + offset + angleInRad / _settings.FOVLineCount * i;

            var x = Mathf.Cos(delta) * _settings.Radius;
            var y = Mathf.Sin(delta) * _settings.Radius;

            var curPos = new Vector3(pos.x + x, pos.y + y, 0);
            linePositions[i + 1] = curPos;
        }
    }

    public void Draw()
    {
        UpdateLinePositions();
        _lineRenderer.SetPositions(linePositions);
    }
}