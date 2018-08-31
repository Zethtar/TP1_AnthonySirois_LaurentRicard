using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Enemy.Strategies
{
    public abstract class Strategy : IEnnemyStrategy
    {
        protected const float MIN_DISTANCE_BETWEEN_ENEMIES = 3f;
        protected readonly Vector3 downRight;
        protected readonly HandController handController;

        protected readonly Mover mover;

        protected readonly Vector3 topLeft;
        protected EnnemyState currentState;
        protected EnemyEnemyMemory enemyMemory;
        protected EnemyPickableMemory pickableMemory;

        protected Vector3 roamingTarget;

        public Strategy(
            Mover mover,
            HandController handController,
            EnemyEnemyMemory enemyMemory,
            EnemyPickableMemory pickableMemory)
        {
            this.mover = mover;
            this.handController = handController;
            this.enemyMemory = enemyMemory;
            this.pickableMemory = pickableMemory;

            roamingTarget = GetRandomLocation();

            topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            downRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            currentState = EnnemyState.Roaming;
        }

        public abstract void Act();

        protected virtual void Think()
        {
            currentState = EnnemyState.Idle;
        }

        protected Vector3 GetRandomLocation()
        {
            var randomLocation = new Vector3(
                Random.Range(topLeft.x, downRight.x),
                Random.Range(downRight.y, topLeft.y),
                0);

            return randomLocation;
        }

        protected bool IsTargetReached(Vector3 target)
        {
            return Vector3.Distance(mover.transform.root.position, target) < 0.1;
        }

        protected void LookingForEnemies()
        {
            if (enemyMemory.GetEnemyTarget() == null && enemyMemory.IsAnEnemyInSight())
                enemyMemory.TargetNearestEnemy(mover.transform.root.position);
        }

        protected void LookingForTypedPickable(PickableCategory pickableType)
        {
            if (pickableMemory.GetPickableTarget() == null && pickableMemory.IsTypePickableInSight(pickableType))
                pickableMemory.TargetNearestTypedPickable(mover.transform.root.position, pickableType);
        }

        protected void MoveTowards(Vector3 targetPosition)
        {
            mover.MoveToTarget(targetPosition);
            mover.RotateToTarget(targetPosition);
        }

        protected void Roaming()
        {
            if (IsTargetReached(roamingTarget)) roamingTarget = GetRandomLocation();

            mover.RotateToTarget(roamingTarget);
            mover.MoveToTarget(roamingTarget);
        }

        protected void SweepingAroundClockwise()
        {
            mover.Rotate(1);
        }
    }
}