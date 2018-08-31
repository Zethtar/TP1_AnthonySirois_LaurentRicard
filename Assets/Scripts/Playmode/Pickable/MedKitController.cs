using System;
using Playmode.Enemy;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{
    public class MedKitController : PickableController
    {
        [Header("Values")] [SerializeField] private int healthPointRestored = 75;

        public MedKitController()
        {
            Category = PickableCategory.Util;
        }

        protected override void ValidateSerialisedFields()
        {
            if (healthPointRestored <= 0)
                throw new ArgumentException("Medkit can't do damage.");
        }

        protected override void OnCollision(EnemyController enemy)
        {
            enemy.Heal(healthPointRestored);

            destroyer.Destroy();
        }
    }
}