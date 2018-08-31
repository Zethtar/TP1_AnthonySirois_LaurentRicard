using System;
using Playmode.Entity.Senses;
using Playmode.Weapon.Types;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        protected int bulletDamage;
        [Header("Behaviour")] [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected int bulletsBaseDamage = 10;
        [SerializeField] protected float fireDelayInSeconds = 1f;
        protected float lastTimeShotInSeconds;
        [SerializeField] protected WeaponType weaponType;

        public WeaponType WeaponType => weaponType;
        protected bool CanShoot => Time.time - lastTimeShotInSeconds > fireDelayInSeconds;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();

            bulletDamage = bulletsBaseDamage;
        }

        private void ValidateSerialisedFields()
        {
            if (fireDelayInSeconds < 0)
                throw new ArgumentException("FireRate can't be lower than 0.");
        }

        private void InitializeComponent()
        {
            lastTimeShotInSeconds = 0;
        }

        public virtual void Shoot()
        {
            if (CanShoot)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponentInChildren<HitStimulus>().HitPoints = bulletDamage;

                lastTimeShotInSeconds = Time.time;
            }
        }

        public virtual void Upgrade()
        {
        }
    }
}