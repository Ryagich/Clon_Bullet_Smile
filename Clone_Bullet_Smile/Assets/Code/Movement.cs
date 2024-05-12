using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public UnityEvent Started;
    public UnityEvent Canceled;
    
    public Vector3 StartPosition { get; private set; }

    [SerializeField] private MovementCalculations _calculations;
    [SerializeField] private Rigidbody _rb;

    private bool canMove;

    public void ChangeMovementState(bool newState)
    {
        canMove = newState;
    }

    public void OnTryMove(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Started:
                StartPosition = _calculations.GetMouseScreenPosition();
                _rb.velocity = Vector3.zero;
                Started?.Invoke();
                break;
            case InputActionPhase.Canceled:
                _calculations.Move(_rb, StartPosition, _calculations.GetMouseScreenPosition());
                Canceled?.Invoke();
                break;
        }
    }
}