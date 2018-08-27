using System;
using Playmode.Entity.Senses;
using Playmode.Weapon.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Behaviour")] [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected float fireDelayInSeconds = 1f;
        [SerializeField] protected int bulletsBaseDamage = 10;
        [SerializeField] protected WeaponType weaponType;
        
        protected int bulletDamage;
        protected float lastTimeShotInSeconds;

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
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponentInChildren<HitStimulus>().HitPoints = bulletDamage;          
                
                lastTimeShotInSeconds = Time.time;
            }
        }

        public virtual void Upgrade()
        {
            
        }
    }
}