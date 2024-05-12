using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent Entered;
    [SerializeField] private Transform _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _target)
        {
            Entered?.Invoke();
        }
    }
}