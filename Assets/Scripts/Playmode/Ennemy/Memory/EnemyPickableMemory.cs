using System.Collections.Generic;
using Playmode.Pickable;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Enemy.Memory
{
    public class EnemyPickableMemory : EnemyMemory
    {
        private readonly ICollection<PickableController> pickablesInSight;

        private PickableController target;

        public EnemyPickableMemory()
        {
            pickablesInSight = new HashSet<PickableController>();
        }

        public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

        public PickableController GetPickableTarget()
        {
            return target;
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

        public bool IsTypePickableInSight(PickableCategory type)
        {
            foreach (var pickable in pickablesInSight)
                if (pickable.Category == type)
                    return true;

            return false;
        }

        public void TargetNearestTypedPickable(Vector3 selfPosition, PickableCategory type)
        {
            PickableController nearestPickable = null;

            foreach (var pickable in pickablesInSight)
                if (pickable != null)
                    if (pickable.Category == type)
                    {
                        if (nearestPickable == null)
                            nearestPickable = pickable;
                        else if (IsVectorCloserThanOtherVector(
                            selfPosition,
                            pickable.transform.root.position,
                            nearestPickable.transform.root.position))
                            nearestPickable = pickable;
                    }

            target = nearestPickable;
        }

        private void OnPickableDestroy(PickableController pickable)
        {
            if (pickable == target) target = null;

            Remove(pickable);
        }
    }
}