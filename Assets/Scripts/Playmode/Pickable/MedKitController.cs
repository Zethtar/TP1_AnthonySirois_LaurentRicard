using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;
using Playmode.Pickable;
using UnityEngine;

namespace Playmode.Pickable
{
    public class MedKitController : PickableController {
    
        [Header("Values")] [SerializeField] private int healthPointRestored = 75;

        protected override void OnCollision(EnnemyController ennemy)
        {
            ennemy.Heal(healthPointRestored);
            
            destroyer.Destroy();
        }
    }
}


