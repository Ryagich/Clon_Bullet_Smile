using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCalculations : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] public float _power = 2;

    public void Move(Rigidbody rb, Vector3 start, Vector3 end)
    {
        rb.AddForce((end - start) * _power, ForceMode.VelocityChange);
    }

    public Vector3 GetMouseScreenPosition()
    {
        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        new Plane(-Vector3.forward, transform.position).Raycast(ray, out var enter);
        return ray.GetPoint(enter);
    }
}