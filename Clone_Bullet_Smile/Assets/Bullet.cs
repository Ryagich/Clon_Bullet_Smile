using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;

    private float speed;
    private int damage;
    
    private Vector3 lastPos;

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
        if (Physics.Linecast(lastPos, transform.position, out var hit, _targetMask))
        {
            var health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.ChangeAmount(-damage);
            }
            Destroy(gameObject);
        }

        lastPos = transform.position;
    }

    public void SetValues(float speed, int damage, float liveTime)
    {
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, liveTime);
    }
}