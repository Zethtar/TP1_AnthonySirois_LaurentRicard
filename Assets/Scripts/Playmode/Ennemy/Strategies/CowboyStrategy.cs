using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CowboyStrategy : Strategy
    {

        public CowboyStrategy(Mover mover, HandController handController) : base(mover, handController)
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
                currentState = EnnemyState.Roaming;
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
            //handController.AimTowards(ennemyTarget.transform.position);
            mover.RotateToTarget(ennemyTarget.transform.position);
            mover.MoveToTarget(ennemyTarget.transform.position);
            handController.Use();
        }

    }
}