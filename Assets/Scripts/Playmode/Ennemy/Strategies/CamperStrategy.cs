using Playmode.Ennemy.BodyParts;
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
            if (emergencyMedkit == null)
            {
                currentState = EnnemyState.MedkitSearching;
            }
            else if (health.HealthPoints < HEALTH_THRESHOLD)
            {
                currentState = EnnemyState.MedkitGathering;
            }
            //else if (ennemyTarget != null)
            //{
            //    currentState = EnnemyState.Attacking;
            //}
            else if(ennemyEnnemyMemory.IsAnEnnemyInSight())
            {
                ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
                currentState = EnnemyState.Attacking;
            }
            else
            {
                currentState = EnnemyState.Idle;
            }
        }

        public override void Act()
        {
            Think();

            if (currentState == EnnemyState.MedkitGathering)
            {
                mover.MoveToTarget(emergencyMedkit.transform.root.position);
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(ennemyTarget);
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
                    Sweep();
                }
            }
            else if(currentState == EnnemyState.MedkitSearching)
            {
                if (ennemyPickableMemory.IsTypePickableInSight(PickableCategory.Util))
                {
                    emergencyMedkit =
                        ennemyPickableMemory.GetNearestTypedPickable(
                            mover.transform.root.position,
                            PickableCategory.Util);
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

        private void Sweep()
        {
            mover.Rotate(1);
        }

    }
}