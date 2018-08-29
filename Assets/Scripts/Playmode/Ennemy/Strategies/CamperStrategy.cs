using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CamperStrategy : Strategy
    {

        public CamperStrategy(
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

        override public void Act()
        {
            if (ennemyTarget != null)
            {
                currentState = EnnemyState.Attacking;
            }
            else
            {
                if (ennemyEnnemyMemory.IsAnEnnemyInSight())
                {
                    ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
                    currentState = EnnemyState.Attacking;
                }
                else
                {
                    currentState = EnnemyState.Roaming;
                }
            }
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