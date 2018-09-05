using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Enemy.Strategies
{
    public class CamperStrategy : Strategy
    {
        private const int HEALTH_THRESHOLD = 50;
        private const float MEDKIT_DISTANCE = 3f;

        private readonly Health health;
        private PickableController emergencyMedkit;

        public CamperStrategy(
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
            if (emergencyMedkit == null)
            {
                currentState = EnnemyState.MedkitSearching;
            }
            else if (health.HealthPoints < HEALTH_THRESHOLD)
            {
                currentState = EnnemyState.MedkitGathering;
            }
            else
            {
                LookingForEnemies();

                currentState = enemyMemory.GetEnemyTarget() != null
                    ? EnnemyState.Attacking
                    : EnnemyState.Camping;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.MedkitGathering)
            {
                MoveTowards(emergencyMedkit.transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(enemyMemory.GetEnemyTarget());
            }
            else if (currentState == EnnemyState.Camping)
            {
                Camping();
            }
            else if (currentState == EnnemyState.MedkitSearching)
            {
                MedkitSearching();
            }
            else if (currentState == EnnemyState.Roaming)
            {
                Roaming();
            }
        }

        private void AttackEnemy(EnemyController enemyTarget)
        {
            mover.RotateToTarget(enemyTarget.transform.root.position);

            handController.Use();
        }

        private void MedkitSearching()
        {
            if (pickableMemory.IsTypePickableInSight(PickableCategory.Util))
            {
                LookingForTypedPickable(PickableCategory.Util);
                emergencyMedkit = pickableMemory.GetPickableTarget();
            }
            else
            {
                Roaming();
            }
        }

        private void Camping()
        {
            if (Vector3.Distance(mover.transform.root.position, emergencyMedkit.transform.root.position) >
                    MEDKIT_DISTANCE)
                mover.MoveToTarget(emergencyMedkit.transform.root.position);
            else
                SweepingAroundClockwise();
        }
    }
}