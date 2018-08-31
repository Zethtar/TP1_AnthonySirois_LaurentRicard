using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Memory;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CowboyStrategy : Strategy
    {
        public CowboyStrategy(
            Mover mover,
            HandController handController,
            EnnemyEnnemyMemory enemyMemory,
            EnnemyPickableMemory pickableMemory)
            : base(
                  mover,
                  handController,
                  enemyMemory,
                  pickableMemory)
        {
        }

        protected override void Think()
        {
            base.LookingForTypedPickable(PickableCategory.Weapon);
            if (pickableMemory.GetPickableTarget() != null)
            {
                currentState = EnnemyState.WeaponGathering;
                
            }
            else
            {
                base.LookingForEnemies();
                currentState = enemyMemory.GetEnemyTarget() != null 
                    ? EnnemyState.Attacking : EnnemyState.Roaming;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.WeaponGathering)
            {
                base.MoveTowards(pickableMemory.GetPickableTarget().transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                ChargeTheEnnemy(enemyMemory.GetEnemyTarget().transform.root.position);
            }
            else if (currentState == EnnemyState.Roaming)
            {
                base.Roaming();
            }
        }

        private void ChargeTheEnnemy(Vector3 enemyPosition)
        {
            mover.RotateToTarget(enemyPosition);
            if ((Vector3.Distance(mover.transform.root.position, enemyPosition)) > MIN_DISTANCE_BETWEEN_ENEMIES)
            {
                mover.MoveToTarget(enemyPosition);
            }
            handController.Use();
        }
 
    }
}