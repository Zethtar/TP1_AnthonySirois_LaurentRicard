using Playmode.Entity.Senses;
using UnityEngine;

namespace Playmode.Weapon
{
    public class ShotgunController : WeaponController
    {
        [SerializeField] private float angleBetweenBullets = 10f;
        [SerializeField] private int nbBullets = 5;

        public override void Shoot()
        {
            if (CanShoot)
            {
                for (var i = 0; i < nbBullets; i++)
                {
                    var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.GetComponentInChildren<HitStimulus>().HitPoints = bulletDamage;
                    bullet.transform.Rotate(Vector3.forward,
                        angleBetweenBullets * i - nbBullets / 2 * angleBetweenBullets);
                }

                lastTimeShotInSeconds = Time.time;
            }
        }

        public override void Upgrade()
        {
            bulletDamage += bulletsBaseDamage / 2;
        }
    }
}