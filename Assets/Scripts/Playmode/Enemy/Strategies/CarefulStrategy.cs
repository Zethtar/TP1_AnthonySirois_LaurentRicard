using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Enemy.Strategies
{
    public class CarefulStrategy : Strategy
    {
        private const float SAFE_DISTANCE = 6f;
        private const float HEALTH_THRESHOLD = 50;
        private readonly Health health;

        public CarefulStrategy(
            Mover mover,
            HandController handController,
            Health health,
            EnemyEnemyMemory enemyMemory,
            EnemyPickableMemory pickableMemory)
            : base(
                mover,
                handController,
                enemyMemory,
                pickableMemory)
        {
            this.health = health;
        }

        protected override void Think()
        {
            if (health.HealthPoints <= HEALTH_THRESHOLD &&
                pickableMemory.IsTypePickableInSight(PickableCategory.Util))
            {
                pickableMemory.TargetNearestTypedPickable(
                    mover.transform.root.position,
                    PickableCategory.Util);

                currentState = EnnemyState.MedkitSearching;
            }
            else
            {
                LookingForEnemies();

                currentState = enemyMemory.GetEnemyTarget() != null
                    ? EnnemyState.Attacking
                    : EnnemyState.Roaming;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.MedkitSearching)
            {
                MoveTowards(pickableMemory.GetPickableTarget().transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(enemyMemory.GetEnemyTarget());
            }
            else if (currentState == EnnemyState.Roaming)
            {
                LookingForTypedPickable(PickableCategory.Util);
                if (pickableMemory.GetPickableTarget() != null)
                    roamingTarget = pickableMemory.GetPickableTarget().transform.root.position;

                Roaming();
            }
        }

        private void AttackEnemy(EnemyController enemyTarget)
        {
            mover.RotateToTarget(enemyTarget.transform.root.position);

            if (Vector3.Distance(mover.transform.root.position, enemyTarget.transform.root.position) >= SAFE_DISTANCE)
                mover.MoveToTarget(enemyTarget.transform.root.position);
            else
                mover.Move(Vector3.down);
            handController.Use();
        }
    }
}