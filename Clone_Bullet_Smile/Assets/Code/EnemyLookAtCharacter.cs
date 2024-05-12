using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyLookAtCharacter : MonoBehaviour
{
    public UnityEvent SeeTarget;
    public UnityEvent DontSeeTarget;

    [SerializeField] private EnemyFoV _fov;
    [SerializeField] private LayerMask _layerMask;
    
    public void CheckCanSeeTarget()
    {
        if (Physics.Linecast(_fov.ShootPoint.position,_fov.Target.position,out var hit,_layerMask)
            && hit.transform && hit.transform == _fov.Target)
        {
            SeeTarget?.Invoke();
        }
        else
        {
            DontSeeTarget?.Invoke();
        }
    }
}
