using System;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Behaviour")] [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireDelayInSeconds = 1f;
        [SerializeField] private int nbBullets = 1;
        [SerializeField] private float angleBetweenBullets = 0f;

        private float lastTimeShotInSeconds;

        private bool CanShoot => Time.time - lastTimeShotInSeconds > fireDelayInSeconds;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
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

        public void Shoot()
        {
            if (CanShoot)
            {
                for (int i = 0; i < nbBullets; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.transform.Rotate(Vector3.forward, angleBetweenBullets * i - nbBullets / 2 * angleBetweenBullets);
                }              
                
                lastTimeShotInSeconds = Time.time;
            }
        }
    }
}