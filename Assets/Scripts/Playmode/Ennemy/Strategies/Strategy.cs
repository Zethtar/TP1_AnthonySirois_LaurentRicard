using UnityEngine;
using System.Collections;
using Playmode.Ennemy.Strategies;
using Playmode.Ennemy;
using Playmode.Weapon;
using Playmode.Movement;
using Playmode.Ennemy.BodyParts;

public class Strategy : IEnnemyStrategy
{
    protected readonly Mover mover;
    protected readonly HandController handController;
    protected EnnemyController ennemyTarget;
    protected WeaponController weaponTarget;
    protected Vector3 roamingTarget;
    protected EnnemyState currentState;
    protected EnnemyState lastState = EnnemyState.Idle;

    public Strategy(Mover mover, HandController handController)
    {
        this.mover = mover;
        this.handController = handController;
        roamingTarget = GetRandomLocation();
    }

    virtual public void Act()
    {
    }

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
        Vector3 randomLocation = new Vector3(Random.Range(-17, 17), Random.Range(-10, 10), 0);
        return randomLocation;
    }

    protected bool IsTargetReached(Vector3 target)
    {
        return ((Vector3.Distance(mover.transform.root.position, target)) < 0.1);
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
