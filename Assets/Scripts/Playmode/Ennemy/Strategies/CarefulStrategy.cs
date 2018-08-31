using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Memory;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CarefulStrategy : Strategy
    {
        private const float SAFE_DISTANCE = 9;
        private const float HEALTH_THRESHOLD = 50;
        private readonly Health health;

        public CarefulStrategy(
            Mover mover,
            HandController handController,
            Health health,
            EnnemyEnnemyMemory ennemyEnnemyMemory,
            EnnemyPickableMemory ennemyPickableMemory)
            : base(
                mover,
                handController,
                ennemyEnnemyMemory,
                ennemyPickableMemory)
        {
            this.health = health;
        }

        protected override void Think()
        {
            if (health.HealthPoints <= HEALTH_THRESHOLD &&
                ennemyPickableMemory.IsTypePickableInSight(PickableCategory.Util))
            {
                pickableTarget = ennemyPickableMemory.GetNearestTypedPickable(
                    mover.transform.root.position,
                    PickableCategory.Util);
                currentState = EnnemyState.MedkitSearching;
            }
            else if (ennemyEnnemyMemory.IsAnEnnemyInSight())
            {
                ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
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

            if (currentState == EnnemyState.MedkitSearching)
            {
                mover.RotateToTarget(pickableTarget.transform.root.position);
                mover.MoveToTarget(pickableTarget.transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(ennemyTarget);
            }
            else if (currentState == EnnemyState.Roaming)
            {
                if (ennemyPickableMemory.IsTypePickableInSight(PickableCategory.Weapon))
                {
                    roamingTarget = ennemyPickableMemory.GetNearestPickable(mover.transform.root.position).transform
                        .root.position;
                }

                base.Roaming();
            }
        }

        private void AttackEnemy(EnnemyController ennemyTarget)
        {
            mover.RotateToTarget(ennemyTarget.transform.root.position);

            if ((Vector3.Distance(mover.transform.root.position, ennemyTarget.transform.root.position)) >=
                SAFE_DISTANCE)
            {
                mover.MoveToTarget(ennemyTarget.transform.root.position);
            }
            else
            {
                var safeDirection = mover.transform.root.position - ennemyTarget.transform.root.position;
                mover.Move(safeDirection);
                Debug.Log("Too Close for comfort");
            }

            handController.Use();
        }
    }
}