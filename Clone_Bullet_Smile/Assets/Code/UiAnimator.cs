using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class UiAnimator : MonoBehaviour
{
    public UnityEvent Showed;
    public UnityEvent Hided;

    [SerializeField] private float AnimationDelay;
    [SerializeField] private float AnimationTime;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isShow;

    public void Show()
    {
        if (isShow)
        {
            return;
        }

        isShow = true;
        DOTween.Sequence()
            .AppendInterval(AnimationDelay)
            .AppendCallback(() =>
            {
                canvasGroup.alpha = 0;
                canvasGroup.gameObject.SetActive(true);
            })
            .Append(DOTween.To(() => canvasGroup.alpha,
                a => canvasGroup.alpha = a, 1, AnimationTime))
            .AppendCallback(() =>
            {
                isShow = false;
                Showed?.Invoke();
            });
    }

    public void Hide()
    {
        if (isShow)
        {
            return;
        }

        isShow = true;
        DOTween.Sequence()
            .AppendInterval(AnimationDelay)
            .AppendCallback(() => canvasGroup.alpha = 1)
            .Append(DOTween.To(() => canvasGroup.alpha,
                a => canvasGroup.alpha = a, 0, AnimationTime))
            .AppendCallback(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                isShow = false;
                Hided?.Invoke();
            });
    }
}