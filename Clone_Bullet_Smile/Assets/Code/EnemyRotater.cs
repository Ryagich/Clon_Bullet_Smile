using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRotater : MonoBehaviour
{
    public UnityEvent Rotate;
    
    [SerializeField] private EnemyFoV _fov;
    [SerializeField] private float _offsetToRotation;
    [SerializeField] private float _timeToLoseTarget;
    [SerializeField] private float _holdTime;
    [SerializeField] private float _speed = 5f;

    private Coroutine rotatingToTarget;
    private Coroutine rotating;
    private Coroutine losingTarget;
    private bool isFollowing;
    private float speed;

    private void Start()
    {
        GetRandomDirection();
        speed = _speed;
    }

    public void ChangeSpeed(bool state)
    {
        speed = state ? _speed : 0;
    }

    public void FollowTarget()
    {
        if (isFollowing)
        {
            return;
        }

        isFollowing = true;
        if (rotating != null)
        {
            StopCoroutine(rotating);
            rotating = null;
        }

        if (losingTarget != null)
        {
            StopCoroutine(losingTarget);
            losingTarget = null;
        }

        rotatingToTarget = StartCoroutine(RotatingToTarget());
    }

    private void GetRandomDirection()
    {
        rotating = StartCoroutine(Rotating(Quaternion.Euler(0, 0, Random.Range(0, 359))));
    }

    private IEnumerator Rotating(Quaternion targetrotation)
    {
        while (Quaternion.Angle(transform.rotation, targetrotation) > _offsetToRotation)
        {
            var t = Time.fixedDeltaTime * speed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, t);
            Rotate?.Invoke();
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(_holdTime);

        rotating = null;
        GetRandomDirection();
    }

    private IEnumerator RotatingToTarget()
    {
        while (true)
        {
            var targetDirection = _fov.Target.position - transform.position;
            transform.right = Vector3.Lerp(transform.right, targetDirection, speed * Time.deltaTime);
            Rotate?.Invoke();
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void LoseTarget()
    {
        isFollowing = false;
        if (rotatingToTarget != null)
        {
            StopCoroutine(rotatingToTarget);
            rotatingToTarget = null;
        }

        if (losingTarget == null && rotating == null)
        {
            losingTarget = StartCoroutine(LosingTarget());
        }
    }

    private IEnumerator LosingTarget()
    {
        yield return new WaitForSeconds(_timeToLoseTarget);
        losingTarget = null;
        GetRandomDirection();
    }
}