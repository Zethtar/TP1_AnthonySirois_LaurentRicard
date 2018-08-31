using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Memory;
using Playmode.Movement;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class NormalStrategy : Strategy
    {

        public NormalStrategy(
            Mover mover,
            HandController handController,
            EnnemyEnnemyMemory ennemyEnnemyMemory,
            EnnemyPickableMemory ennemyPickableMemory)
            : base(
                  mover,
                  handController,
                  ennemyEnnemyMemory,
                  ennemyPickableMemory)
        {
        }

        protected override void Think()
        {
            base.LookingForEnemies();
            if (ennemyTarget != null)
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
            {
                ChargeTheEnnemy(ennemyTarget);
            }
            else if (currentState == EnnemyState.Roaming)
            {
                base.Roaming();
            }
        }

        private void ChargeTheEnnemy(EnnemyController ennemyTarget)
        {
            mover.RotateToTarget(ennemyTarget.transform.root.position);

            if ((Vector3.Distance(mover.transform.root.position, ennemyTarget.transform.root.position)) > 3)
            {
                mover.MoveToTarget(ennemyTarget.transform.root.position);
            }
            handController.Use();
        }

    }
}