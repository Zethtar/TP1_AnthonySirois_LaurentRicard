using UnityEngine;
using Playmode.Ennemy.Strategies;
using Playmode.Ennemy;
using Playmode.Movement;
using Playmode.Ennemy.BodyParts;
using Playmode.Pickable;
using Playmode.Ennemy.Memory;

public abstract class Strategy : IEnnemyStrategy
{
    protected readonly Mover mover;
    protected readonly HandController handController;
    protected EnnemyController ennemyTarget;
    protected PickableController pickableTarget;
    protected Vector3 roamingTarget;
    protected EnnemyState currentState;
    protected EnnemyState lastState = EnnemyState.Idle;

    protected EnnemyEnnemyMemory ennemyEnnemyMemory;
    protected EnnemyPickableMemory ennemyPickableMemory;

    protected readonly Vector3 topLeft;
    protected readonly Vector3 downRight;

    public Strategy(
        Mover mover, 
        HandController handController, 
        EnnemyEnnemyMemory ennemyEnnemyMemory, 
        EnnemyPickableMemory ennemyPickableMemory)
    {
        this.mover = mover;
        this.handController = handController;
        this.ennemyEnnemyMemory = ennemyEnnemyMemory;
        this.ennemyPickableMemory = ennemyPickableMemory;
        roamingTarget = GetRandomLocation();
        
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        downRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); 
    }

    public abstract void Act();

    protected virtual void Think()
    {
        currentState = EnnemyState.Idle;
    }

    public void SetState(EnnemyState state)
    {
        currentState = state;
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
        if (ennemyEnnemyMemory.GetEnnemyTarget() == null && ennemyEnnemyMemory.IsAnEnnemyInSight())
        {
            ennemyTarget = ennemyEnnemyMemory.GetNearestEnnemy(mover.transform.root.position);
        }
        else
        {
            ennemyTarget = ennemyEnnemyMemory.GetEnnemyTarget();
        }
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
}
