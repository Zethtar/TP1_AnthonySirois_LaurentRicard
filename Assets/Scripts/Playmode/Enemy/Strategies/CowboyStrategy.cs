using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Enemy.Strategies
{
    public class CowboyStrategy : Strategy
    {
        public CowboyStrategy(
            Mover mover,
            HandController handController,
            EnemyEnemyMemory enemyMemory,
            EnemyPickableMemory pickableMemory)
            : base(
                mover,
                handController,
                enemyMemory,
                pickableMemory)
        {
        }

        protected override void Think()
        {
            LookingForTypedPickable(PickableCategory.Weapon);
            if (pickableMemory.GetPickableTarget() != null)
            {
                currentState = EnnemyState.WeaponGathering;
            }
            else
            {
                LookingForEnemies();
                currentState = enemyMemory.GetEnemyTarget() != null
                    ? EnnemyState.Attacking
                    : EnnemyState.Roaming;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.WeaponGathering)
                MoveTowards(pickableMemory.GetPickableTarget().transform.root.position);
            else if (currentState == EnnemyState.Attacking)
                ChargeTheEnnemy(enemyMemory.GetEnemyTarget().transform.root.position);
            else if (currentState == EnnemyState.Roaming) Roaming();
        }

        private void ChargeTheEnnemy(Vector3 enemyPosition)
        {
            mover.RotateToTarget(enemyPosition);
            if (Vector3.Distance(mover.transform.root.position, enemyPosition) > MIN_DISTANCE_BETWEEN_ENEMIES)
                mover.MoveToTarget(enemyPosition);
            handController.Use();
        }
    }
}