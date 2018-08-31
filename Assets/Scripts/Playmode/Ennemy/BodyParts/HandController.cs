using System;
using Playmode.Movement;
using Playmode.Weapon;
using Playmode.Weapon.Types;
using UnityEngine;

namespace Playmode.Ennemy.BodyParts
{
    public class HandController : MonoBehaviour
    {
        private Mover mover;
        private WeaponController weapon;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mover = GetComponent<AnchoredMover>();
        }

        public void Hold(GameObject gameObject)
        {
            if (gameObject != null)
            {
                if (gameObject.GetComponentInChildren<WeaponController>().WeaponType == WeaponType.Uzi &&
                    GetComponentInChildren<UziController>() != null)
                {
                    weapon = GetComponentInChildren<UziController>();
                    weapon.Upgrade();
                    
                    Destroy(gameObject);
                }
                else if (gameObject.GetComponentInChildren<WeaponController>().WeaponType == WeaponType.Shotgun &&
                         GetComponentInChildren<ShotgunController>() != null)
                {
                    weapon = GetComponentInChildren<ShotgunController>();
                    weapon.Upgrade();
                    
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.transform.parent = transform;
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.rotation = transform.rotation;
                                        
                    weapon = gameObject.GetComponentInChildren<WeaponController>();
                }
            }
            else
            {
                weapon = null;
            }
        }

        public void AimTowards(Vector3 target)
        {
        }

        public void Use()
        {
            if (weapon != null) weapon.Shoot();
        }
    }
}