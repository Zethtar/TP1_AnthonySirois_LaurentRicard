﻿using Playmode.Entity.Senses;
using UnityEngine;

namespace Playmode.Weapon
{
    public class UziController : WeaponController
    {
        [SerializeField] private float spread = 5f;

        public override void Shoot()
        {
            if (CanShoot)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                bullet.GetComponentInChildren<HitStimulus>().HitPoints = bulletDamage;
                bullet.transform.Rotate(Vector3.forward, Random.Range(-spread / 2, spread));

                lastTimeShotInSeconds = Time.time;
            }
        }

        public override void Upgrade()
        {
            bulletDamage += bulletsBaseDamage / 2;
            fireDelayInSeconds /= 1.25f;

            if (spread < 45f) spread *= 1.15f;
        }
    }
}