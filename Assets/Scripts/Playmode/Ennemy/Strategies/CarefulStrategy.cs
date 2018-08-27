﻿using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CarfulStrategy : Strategy
    {

        public CarfulStrategy(Mover mover, HandController handController) : base(mover, handController)
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
            if ((Vector3.Distance(mover.transform.root.position, ennemyTarget.transform.position)) > 3)
            {
                mover.MoveToTarget(ennemyTarget.transform.position);
            }
            handController.Use();
        }

    }
}