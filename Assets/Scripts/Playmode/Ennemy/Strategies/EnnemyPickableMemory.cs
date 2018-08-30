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
        pickable.OnPickableDestroy += OnPickableDestroy;
    }

    public void Remove(PickableController pickable)
    {
        pickablesInSight.Remove(pickable);
        pickable.OnPickableDestroy -= OnPickableDestroy;
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
            else if (IsPickableNearestThanOtherPickable(selfPosition, nearestPickable.transform.root.position, pickable.transform.root.position))
            {
                nearestPickable = pickable;
            }
        }
        return nearestPickable;
    }

    private bool IsPickableNearestThanOtherPickable(Vector3 selfPosition, Vector3 firstEnnemyPosition, Vector3 secondEnnemyPosition)
    {
        return ((Vector3.Distance(selfPosition, firstEnnemyPosition)) <
            (Vector3.Distance(selfPosition, secondEnnemyPosition)));
    }

    private void OnPickableDestroy(PickableController pickable)
    {
        Remove(pickable);
    }

}
