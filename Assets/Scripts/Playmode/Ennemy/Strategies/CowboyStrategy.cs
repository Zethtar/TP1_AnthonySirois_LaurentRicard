﻿using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable.Types;
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
            if (pickableTarget == null)
            {
                LookingForWeapons();
            }

            if (pickableTarget == null)
            {
                LookingForEnemies();
            }

            if (ennemyTarget == null && pickableTarget == null)
            {
                currentState = EnnemyState.Roaming;
            }

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
            if (ennemyPickableMemory.IsTypePickableInSight(PickableCategory.Weapon))
            {
                pickableTarget = ennemyPickableMemory.GetNearestTypedPickable(mover.transform.root.position, PickableCategory.Weapon);
                currentState = EnnemyState.WeaponSearching;
            }
        }

        private void LookingForEnemies()
        {
            if (ennemyEnnemyMemory.IsAnEnnemyInSight())
            {
                ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
                currentState = EnnemyState.Attacking;
            }
        }

    }
}