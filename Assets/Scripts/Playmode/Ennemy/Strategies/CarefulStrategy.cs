using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CarfulStrategy : IEnnemyStrategy
    {
        private readonly Mover mover;
        private readonly HandController handController;
        private EnnemyController ennemyTarget;
        private WeaponController weaponTarget;
        private Vector3 roamingTarget;
        private EnnemyState currentState;
        private EnnemyState lastState = EnnemyState.Idle;

        public CarfulStrategy(Mover mover, HandController handController)
        {
            this.mover = mover;
            this.handController = handController;
            roamingTarget = GetRandomLocation();

        }

        public void Act()
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
                Roaming();
            }
        }

        public void SetEnnemyTarget(EnnemyController ennemyTarget)
        {
            this.ennemyTarget = ennemyTarget;
        }

        public void SetWeaponTarget(WeaponController weaponTarget)
        {
            this.weaponTarget = weaponTarget;
        }

        public void SetState(EnnemyState state)
        {
            this.currentState = state;
        }

        private Vector3 GetRandomLocation()
        {
            Vector3 randomLocation = new Vector3(Random.Range(-17, 17), Random.Range(-10, 10), 0);
            return randomLocation;
        }

        private bool IsTargetReached(Vector3 target)
        {
            return (Vector3.Distance(mover.transform.root.position, target) < 0.1);
        }

        private void Roaming()
        {
            if (IsTargetReached(roamingTarget))
            {
                roamingTarget = GetRandomLocation();
            }
            mover.RotateToTarget(roamingTarget);
            mover.MoveToTarget(roamingTarget);
            
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