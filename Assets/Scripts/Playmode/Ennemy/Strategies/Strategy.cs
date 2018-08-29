using UnityEngine;
using System.Collections;
using Playmode.Ennemy.Strategies;
using Playmode.Ennemy;
using Playmode.Weapon;
using Playmode.Movement;
using Playmode.Ennemy.BodyParts;

public abstract class Strategy : IEnnemyStrategy
{
    protected readonly Mover mover;
    protected readonly HandController handController;
    protected EnnemyController ennemyTarget;
    protected WeaponController weaponTarget;
    protected Vector3 roamingTarget;
    protected EnnemyState currentState;
    protected EnnemyState lastState = EnnemyState.Idle;

    protected readonly Vector3 topLeft;
    protected readonly Vector3 downRight;

    public Strategy(Mover mover, HandController handController)
    {
        this.mover = mover;
        this.handController = handController;
        roamingTarget = GetRandomLocation();
        
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        downRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); 
    }

    public abstract void Act();

    protected abstract void Think();

    public void SetEnnemyTarget(EnnemyController ennemyTarget)
    {
        this.ennemyTarget = ennemyTarget;
    }

    public void SetState(EnnemyState state)
    {
        currentState = state;
    }

    public void SetWeaponTarget(WeaponController weaponTarget)
    {
        this.weaponTarget = weaponTarget;
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
