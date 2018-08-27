using Playmode.Entity.Senses;
using UnityEngine;

namespace Playmode.Weapon
{
    public class ShotgunController : WeaponController
    {
        [SerializeField] private int nbBullets = 5;
        [SerializeField] private float angleBetweenBullets = 10f;
        
        public override void Shoot()
        {
            if (CanShoot)
            {
                for (int i = 0; i < nbBullets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.GetComponentInChildren<HitStimulus>().HitPoints = bulletDamage;
                    bullet.transform.Rotate(Vector3.forward, angleBetweenBullets * i - nbBullets / 2 * angleBetweenBullets);
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