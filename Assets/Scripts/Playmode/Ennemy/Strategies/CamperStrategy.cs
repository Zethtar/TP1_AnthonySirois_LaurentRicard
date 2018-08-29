using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CamperStrategy : Strategy
    {
        private readonly Health health;
        
        public CamperStrategy(Mover mover, HandController handController, Health health) : base(mover, handController)
        {
            this.health = health;
        }

        protected override void Think()
        {
            if (ennemyTarget != null)
            {
                currentState = EnnemyState.Attacking;
            }
            else
            {
                currentState = EnnemyState.Roaming;
            }
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
            //handController.AimTowards(ennemyTarget.transform.position);
            mover.RotateToTarget(ennemyTarget.transform.position);
            if ((Vector3.Distance(mover.transform.root.position, ennemyTarget.transform.position)) > 3)
            {
                mover.MoveToTarget(ennemyTarget.transform.position);
            }
            handController.Use();
        }

    }
}