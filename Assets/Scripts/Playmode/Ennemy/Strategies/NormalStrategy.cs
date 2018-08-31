using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Movement;
using UnityEngine;

namespace Playmode.Enemy.Strategies
{
    public class NormalStrategy : Strategy
    {
        public NormalStrategy(
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
            LookingForEnemies();
            if (enemyMemory.GetEnemyTarget() != null)
            {
                currentState = EnnemyState.Attacking;
                return;
            }

            currentState = EnnemyState.Roaming;
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.Attacking)
                ChargeTheEnnemy(enemyMemory.GetEnemyTarget());
            else if (currentState == EnnemyState.Roaming) Roaming();
        }

        private void ChargeTheEnnemy(EnemyController enemyTarget)
        {
            mover.RotateToTarget(enemyTarget.transform.root.position);

            if (Vector3.Distance(mover.transform.root.position, enemyTarget.transform.root.position) > 3)
                mover.MoveToTarget(enemyTarget.transform.root.position);
            handController.Use();
        }
    }
}