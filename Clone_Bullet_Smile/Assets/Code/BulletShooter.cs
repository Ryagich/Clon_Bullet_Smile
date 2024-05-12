using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private TurretSettings _settings;
    [SerializeField] private Bullet _pref;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletLiveTime = 3f;

    public void Shoot()
    {
        var bullet = Instantiate(_pref, _parent.position, _parent.rotation);
        bullet.SetValues(_speed, -_settings.Damage, _bulletLiveTime);
    }
}