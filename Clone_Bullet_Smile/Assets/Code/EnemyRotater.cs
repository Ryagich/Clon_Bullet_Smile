using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRotater : MonoBehaviour
{
    public UnityEvent Rotate;

    [SerializeField] private TurretSettings _settings;

    private Coroutine currentCoroutine;
    private bool isFollowing;
    private float speed;

    private void Start()
    {
        StartRandomRotation();
        EnableRotation(true);
    }

    public void EnableRotation(bool state)
    {
        speed = state ? _settings.RotateSpeed : 0;
    }

    private void SetCurrentCoroutine(IEnumerator coroutine)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(coroutine);
    }

    public void FollowTarget()
    {
        if (isFollowing)
        {
            return;
        }
        isFollowing = true;

        SetCurrentCoroutine(RotatingToTarget());
    }

    private void StartRandomRotation()
    {
        SetCurrentCoroutine(Rotating(Quaternion.Euler(0, 0, Random.Range(0, 359))));
    }

    private IEnumerator Rotating(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > _settings.OffsetToRotation)
        {
            var t = Time.deltaTime * speed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            Rotate?.Invoke();
            yield return null;
        }

        yield return new WaitForSeconds(_settings.RotateHoldTime);

        StartRandomRotation();
    }

    private IEnumerator RotatingToTarget()
    {
        while (true)
        {
            var targetDirection = Target.Instance.transform.position - transform.position;
            transform.right = Vector3.MoveTowards(transform.right, targetDirection, speed * Time.deltaTime);
            Rotate?.Invoke();
            yield return null;
        }
    }

    public void LoseTarget()
    {
        if (isFollowing)
        {
            isFollowing = false;
            SetCurrentCoroutine(LosingTarget());
        }
    }

    private IEnumerator LosingTarget()
    {
        yield return new WaitForSeconds(_settings.TimeToLoseTarget);
        StartRandomRotation();
    }
}