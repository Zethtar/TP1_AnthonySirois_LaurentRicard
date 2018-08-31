using UnityEngine;
using Playmode.Ennemy.Strategies;
using Playmode.Ennemy;
using Playmode.Movement;
using Playmode.Ennemy.BodyParts;
using Playmode.Pickable;
using Playmode.Ennemy.Memory;
using Playmode.Pickable.Types;

public abstract class Strategy : IEnnemyStrategy
{
    protected const float MIN_DISTANCE_BETWEEN_ENEMIES = 3f;
    
    protected readonly Mover mover;
    protected readonly HandController handController;
    protected EnnemyEnnemyMemory enemyMemory;
    protected EnnemyPickableMemory pickableMemory;
    
    protected Vector3 roamingTarget;
    protected EnnemyState currentState;

    protected readonly Vector3 topLeft;
    protected readonly Vector3 downRight;

    public Strategy(
        Mover mover, 
        HandController handController, 
        EnnemyEnnemyMemory enemyMemory, 
        EnnemyPickableMemory pickableMemory)
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
        Vector3 randomLocation = new Vector3(
            Random.Range(topLeft.x, downRight.x),
            Random.Range(downRight.y, topLeft.y),
            0);
        
        return randomLocation;
    }

    protected bool IsTargetReached(Vector3 target)
    {
        return (Vector3.Distance(mover.transform.root.position, target) < 0.1);
    }

    protected void LookingForEnemies()
    {
        if (enemyMemory.GetEnemyTarget() == null && enemyMemory.IsAnEnemyInSight())
        {
            enemyMemory.TargetNearestEnemy(mover.transform.root.position);
        }
    }

    protected void LookingForTypedPickable(PickableCategory pickableType)
    {
        if (pickableMemory.GetPickableTarget() == null && pickableMemory.IsTypePickableInSight(pickableType))
        {
            pickableMemory.TargetNearestTypedPickable(mover.transform.root.position, pickableType);
        }
    }

    protected void MoveTowards(Vector3 targetPosition)
    {
        mover.MoveToTarget(targetPosition);
        mover.RotateToTarget(targetPosition);
    }

    protected void Roaming()
    {
        if (IsTargetReached(roamingTarget))
        {
            roamingTarget = GetRandomLocation(); 
        }
        
        mover.RotateToTarget(roamingTarget);
        mover.MoveToTarget(roamingTarget);
    }

    protected void SweepingAroundClockwise()
    {
        mover.Rotate(1);
    }
}
