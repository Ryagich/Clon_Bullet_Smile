using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(.0f, 1f)] private float _speed = 2f;
    [SerializeField, Range(.0f, 1f)] private float _minScale = .2f;

    private float target = 1f;
    private Coroutine coroutine;
    private float factor;
    private bool canControlTime;
    
    public void ChangeTimeControlState(bool newState)
    {
        canControlTime = newState;
        if (!newState)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            target = 1f;
            Time.timeScale = 1;
        }
    } 
    
    public void OnTryChangeTimeScale(InputAction.CallbackContext context)
    {
        if (!canControlTime)
        {
            return;
        }
        
        switch (context.phase)
        {
            case InputActionPhase.Started:
                target = _minScale;
                factor = -1;
                StartTimeChanging();
                break;
            case InputActionPhase.Canceled:
                target = 1f;
                factor = 1;
                StartTimeChanging();
                break;
        }
    }

    private void StartTimeChanging()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(TimeChanging());
        }
    }

    private IEnumerator TimeChanging()
    {
        while (Time.timeScale != target)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale + (_speed * factor), _minScale, 1);
            yield return null;
        }
        coroutine = null;
    }
}