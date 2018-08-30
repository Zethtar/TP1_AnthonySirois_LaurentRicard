using UnityEngine;
using System.Collections;
using Playmode.Pickable;
using System.Collections.Generic;

public class EnnemyPickableMemory : MonoBehaviour
{

    private ICollection<PickableController> pickablesInSight;

    public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

    public EnnemyPickableMemory  ()
    {
        pickablesInSight = new HashSet<PickableController>();
    }

    public void See(PickableController pickable)
    {
        pickablesInSight.Add(pickable);
    }

    public void LooseSightOf(PickableController pickable)
    {
        pickablesInSight.Remove(pickable);
    }

    public bool IsAPickableInSight()
    {
        return (pickablesInSight.Count > 0);
    }

    public PickableController GetNearestPickable()
    {
        PickableController nearestPickable = null;

        foreach (PickableController pickable in pickablesInSight)
        {
            if (nearestPickable == null)
            {
                nearestPickable = pickable;
            }
            else if (IsPickableNearestThanOtherPickable(nearestPickable.transform.root.position, pickable.transform.root.position))
            {
                nearestPickable = pickable;
            }
        }
        return nearestPickable;
    }

    private bool IsPickableNearestThanOtherPickable(Vector3 firstEnnemyPosition, Vector3 secondEnnemyPosition)
    {
        return ((Vector3.Distance(transform.position, firstEnnemyPosition)) <
            (Vector3.Distance(transform.position, secondEnnemyPosition)));
    }
}
