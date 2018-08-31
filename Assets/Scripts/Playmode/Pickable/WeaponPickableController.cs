using System;
using Playmode.Enemy;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{
    public class WeaponPickableController : PickableController
    {
        [Header("Values")] [SerializeField] private GameObject weaponPrefab;

        public WeaponPickableController()
        {
            Category = PickableCategory.Weapon;
        }

        protected override void ValidateSerialisedFields()
        {
            if (weaponPrefab == null)
                throw new ArgumentException("Can't equip null weapon");
        }

        protected override void OnCollision(EnemyController enemy)
        {
            enemy.Equip(Instantiate(
                weaponPrefab,
                Vector3.zero,
                Quaternion.identity));

            destroyer.Destroy();
        }
    }
}