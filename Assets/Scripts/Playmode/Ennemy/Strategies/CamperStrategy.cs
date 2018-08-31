using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Memory;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable;
using Playmode.Pickable.Types;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
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
            EnnemyEnnemyMemory enemyMemory,
            EnnemyPickableMemory pickableMemory)
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
                base.LookingForEnemies();
                
                currentState = enemyMemory.GetEnemyTarget() != null 
                    ? EnnemyState.Attacking : EnnemyState.Idle;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.MedkitGathering)
            {
                base.MoveTowards(emergencyMedkit.transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(enemyMemory.GetEnemyTarget());
            }
            else if (currentState == EnnemyState.Idle)
            {
                if (Vector3.Distance(mover.transform.root.position, emergencyMedkit.transform.root.position) >
                    MEDKIT_DISTANCE)
                {
                    mover.MoveToTarget(emergencyMedkit.transform.root.position);
                }
                else
                {
                    base.SweepingAroundClockwise();
                }
            }
            else if (currentState == EnnemyState.MedkitSearching)
            {
                if (pickableMemory.IsTypePickableInSight(PickableCategory.Util))
                {
                    pickableMemory.TargetNearestTypedPickable(
                         mover.transform.root.position,
                         PickableCategory.Util);
                    emergencyMedkit = pickableMemory.GetPickableTarget();
                }
                else
                {
                    base.Roaming();
                }
            }
            else if (currentState == EnnemyState.Roaming)
            {
                base.Roaming();
            }
        }

        private void AttackEnemy(EnnemyController ennemyTarget)
        {
            mover.RotateToTarget(ennemyTarget.transform.root.position);

            handController.Use();
        }

    }
}