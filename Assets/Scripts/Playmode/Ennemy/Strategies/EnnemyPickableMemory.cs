using UnityEngine;
using System.Collections;
using Playmode.Pickable;
using System.Collections.Generic;

public class EnnemyPickableMemory
{
    private ICollection<PickableController> pickablesInSight;

    public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

    public EnnemyPickableMemory()
    {
        pickablesInSight = new HashSet<PickableController>();
    }

    public void Add(PickableController pickable)
    {
        pickablesInSight.Add(pickable);
    }

    public void Remove(PickableController pickable)
    {
        pickablesInSight.Remove(pickable);
    }

    public bool IsAPickableInSight()
    {
        return (pickablesInSight.Count > 0);
    }

    public PickableController GetNearestPickable(Vector3 selfPosition)
    {
        PickableController nearestPickable = null;

        foreach (PickableController pickable in pickablesInSight)
        {
            if (nearestPickable == null)
            {
                nearestPickable = pickable;
            }
            else if (IsPickableNearestThanOtherPickable(selfPosition, nearestPickable, pickable))
            {
                nearestPickable = pickable;
            }
        }
        return nearestPickable;
    }

    private bool IsPickableNearestThanOtherPickable(Vector3 selfPosition, PickableController firstEnnemy, PickableController secondEnnemy)
    {
        return ((Vector3.Distance(selfPosition, firstEnnemy.transform.position)) <
            (Vector3.Distance(selfPosition, secondEnnemy.transform.position)));
    }
}
