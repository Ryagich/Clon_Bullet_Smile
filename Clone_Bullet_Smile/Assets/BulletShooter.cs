using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private Bullet _pref;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _speed;
    [SerializeField, Min(0)] private int _damage = 1;
    [SerializeField] private float _bulletLiveTime = 3f;

    public void Shoot()
    {
        var bullet = Instantiate(_pref, _parent.position, _parent.rotation);
        bullet.SetValues(_speed, _damage,_bulletLiveTime);
    }
}