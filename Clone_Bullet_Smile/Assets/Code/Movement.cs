using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public UnityEvent Started;
    public UnityEvent Canceled;
    public Vector3 startPos { get; private set; }

    [SerializeField] private MovementCalculations _calculations;
    [SerializeField] private Rigidbody _rb;

    public void OnTryMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                startPos = _calculations.GetMouseScreenPosition();
                _rb.velocity = Vector3.zero;
                Started?.Invoke();
                break;
            case InputActionPhase.Canceled:
                _calculations.Move(_rb, startPos, _calculations.GetMouseScreenPosition());
                Canceled?.Invoke();
                break;
        }
    }

    
}