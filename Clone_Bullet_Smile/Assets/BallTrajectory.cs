using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallTrajectory : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer; //Ссылка на LineRenderer
    [SerializeField] int lineCount; //Количество линий, чем больше, тем более гладкая получиться линия
    [SerializeField] float _lineTimeInterval = .1f; //Длинна линии
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Movement _movement;
    [SerializeField] private GameObject _predict;

    private bool isRendering = false;

    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        var predict = Instantiate(_predict, transform.position, transform.rotation);
        var d = (_movement.GetMouseScreenPosition() - _movement.startPos).normalized;

        var predictRB = predict.GetComponent<Rigidbody>();
        predictRB.AddForce(_rb.velocity);
        predictRB.AddForce(d * _movement._power);

        Physics.simulationMode = SimulationMode.Script;
        
        var points = new Vector3[lineCount];
        lineRenderer.positionCount = lineCount;
        
        for (var i = 0; i < points.Length; i++)
        {
            Physics.Simulate(_lineTimeInterval);
            points[i] = predict.transform.position;
        }
        Physics.simulationMode = SimulationMode.FixedUpdate;

        lineRenderer.SetPositions(points);
        Destroy(predictRB.gameObject);
    }

    public void ChangeRenderingMode(bool newMode)
    {
        isRendering = newMode;
    }

    private void Update()
    {
        if (!isRendering)
        {
            return;
        }

        var speed = (_movement.GetMouseScreenPosition() - transform.position);
        ShowTrajectory(transform.position, speed);
    }
}