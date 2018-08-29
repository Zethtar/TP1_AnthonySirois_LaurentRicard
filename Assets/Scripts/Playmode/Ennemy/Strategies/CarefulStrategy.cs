using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class CarefulStrategy : Strategy
    {
        private const float SAFE_DISTANCE = 6;
        private const float HEALTH_THRESHOLD = 30;
        private readonly Health health;
        
        public CarefulStrategy(Mover mover, HandController handController, Health health) : base(mover, handController)
        {
            this.health = health;
        }

        protected override void Think()
        {
            if (health.HealthPoints <= HEALTH_THRESHOLD)
            {
                currentState = EnnemyState.MedkitSearching;
            }     
            else if (ennemyTarget != null)
            {
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
                base.Roaming();
            }
            else if (currentState == EnnemyState.Attacking)
            {
                AttackEnemy(ennemyTarget);
            }
            else if (currentState == EnnemyState.Roaming)
            {
                base.Roaming();
            }
        }

        private void AttackEnemy(EnnemyController ennemyTarget)
        {
            mover.RotateToTarget(ennemyTarget.transform.root.position);   

            if ((Vector3.Distance(mover.transform.root.position, ennemyTarget.transform.root.position)) > SAFE_DISTANCE)
            {
                mover.MoveToTarget(ennemyTarget.transform.root.position);
            }
            else 
            {
                mover.MoveToTarget(-ennemyTarget.transform.root.position);
            }
            handController.Use();
        }
    }
}