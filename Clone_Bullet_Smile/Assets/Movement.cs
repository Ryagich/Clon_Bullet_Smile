using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] public float _power = 10;
    [SerializeField] private BallTrajectory _trajectory;
    
    public Vector3 startPos;
    private Vector3 endPos;

    public void OnTryMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                startPos = GetMouseScreenPosition();
                _trajectory.ChangeRenderingMode(true);
                break;
            case InputActionPhase.Canceled:
                endPos = GetMouseScreenPosition();
                _rb.AddForce((endPos - startPos).normalized * _power);
                _trajectory.ChangeRenderingMode(false);
                break;
        }
    }

    public Vector3 GetMouseScreenPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        new Plane(-Vector3.forward, transform.position).Raycast(ray, out var enter);
        return ray.GetPoint(enter);
    }
}