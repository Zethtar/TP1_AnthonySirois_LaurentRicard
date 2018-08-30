using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CowboyStrategy : Strategy
    {      
        public CowboyStrategy(
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
            if(weaponTarget != null)
            {
                currentState = EnnemyState.WeaponSearching;

            }
            else
            {
                if (ennemyPickableMemory.IsAPickableInSight())
                {
                    ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
                    currentState = EnnemyState.Attacking;
                }
            }
            else if (ennemyTarget != null)
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