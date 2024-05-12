using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    [SerializeField] [Range(.0f, 1f)] private float _animationTime = 2f;
    [SerializeField] [Range(.0f, 1f)] private float _minScale = .2f;

    private Tween timeScaleTween;
    private bool canControlTime;

    public void EnableTimeControl(bool newState)
    {
        canControlTime = newState;
        if (!newState)
        {
            AnimateTimeScale(1f);
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
                AnimateTimeScale(_minScale);
                break;
            case InputActionPhase.Canceled:
                AnimateTimeScale(1f);
                break;
        }
    }

    private void AnimateTimeScale(float animateTo)
    {
        timeScaleTween?.Kill(false);
        timeScaleTween = DOTween.To(() => Time.timeScale, s => Time.timeScale = s,
            animateTo, _animationTime).SetUpdate(true).SetEase(Ease.Linear).Play();
    }
}