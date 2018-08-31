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

            LookingForWeapons();
            if (pickableTarget != null)
            {
                currentState = EnnemyState.WeaponSearching;
                return;
            }

            LookingForEnemies();
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

            if (currentState == EnnemyState.WeaponSearching)
            {
                GoToWeapon();
            }
            else if (currentState == EnnemyState.Attacking)
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

        private void GoToWeapon()
        {
            mover.RotateToTarget(pickableTarget.transform.root.position);
            mover.MoveToTarget(pickableTarget.transform.root.position);
        }

        private void LookingForWeapons()
        {
            if (ennemyPickableMemory.GetPickableTarget() == null && ennemyPickableMemory.IsTypePickableInSight(PickableCategory.Weapon))
            {
                pickableTarget = ennemyPickableMemory.GetNearestTypedPickable(mover.transform.root.position, PickableCategory.Weapon);
            }
            else
            {
                pickableTarget = ennemyPickableMemory.GetPickableTarget();
            }
        }

        private void LookingForEnemies()
        {
            if (ennemyEnnemyMemory.GetEnnemyTarget() == null && ennemyEnnemyMemory.IsAnEnnemyInSight())
            {
                ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
            }
            else
            {
                ennemyTarget = ennemyEnnemyMemory.GetEnnemyTarget();
            }
        }

    }
}