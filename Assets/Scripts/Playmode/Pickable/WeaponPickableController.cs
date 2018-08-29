using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;
using Playmode.Pickable;
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
        
        protected override void OnCollision(EnnemyController ennemy)
        {
            ennemy.Equip(Instantiate(
                weaponPrefab,
                Vector3.zero,
                Quaternion.identity));
            
            destroyer.Destroy();
        }
    }
}