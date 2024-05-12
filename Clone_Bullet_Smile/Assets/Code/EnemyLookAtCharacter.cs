using UnityEngine;
using UnityEngine.Events;

public class EnemyLookAtCharacter : MonoBehaviour
{
    public UnityEvent SeeTarget;
    public UnityEvent DontSeeTarget;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _shootPoint;

    public void CheckCanSeeTarget()
    {
        var target = Target.Instance.transform;
        if (Physics.Linecast(_shootPoint.position, target.position, out var hit, _layerMask)
            && hit.transform == target)
        {
            SeeTarget?.Invoke();
        }
        else
        {
            DontSeeTarget?.Invoke();
        }
    }
}