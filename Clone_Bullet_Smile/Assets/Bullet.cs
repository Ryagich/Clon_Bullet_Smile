using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int damage;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.transform.GetComponent<Health>();
        if (health)
        {
            health.ChangeAmount(-damage);
        }

        Destroy(gameObject);
    }

    public void SetValues(float speed, int damage, float liveTime)
    {
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, liveTime);
    }
}