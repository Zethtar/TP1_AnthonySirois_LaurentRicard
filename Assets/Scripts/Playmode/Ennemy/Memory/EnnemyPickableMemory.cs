using UnityEngine;
using System.Collections;
using Playmode.Pickable;
using System.Collections.Generic;
using Playmode.Pickable.Types;

namespace Playmode.Ennemy.Memory
{
    public class EnnemyPickableMemory : EnnemyMemory
    {
        private ICollection<PickableController> pickablesInSight;

        public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

        private PickableController pickableTarget = null;

        public EnnemyPickableMemory()
        {
            pickablesInSight = new HashSet<PickableController>();
        }

        public PickableController GetPickableTarget()
        {
            return pickableTarget;
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

        public bool IsTypePickableInSight(PickableCategory type)
        {
            foreach (var pickable in pickablesInSight)
            {
                if (pickable.Category == type)
                    return true;
            }

            return false;
        }

        public PickableController GetNearestPickable(Vector3 selfPosition)
        {
            PickableController nearestPickable = null;

            foreach (var pickable in pickablesInSight)
            {
                if (pickable != null)
                {
                    if (nearestPickable == null)
                    {
                        nearestPickable = pickable;
                    }
                    else if (base.IsVectorCloserThanOtherVector(
                        selfPosition,
                        pickable.transform.root.position,
                        nearestPickable.transform.root.position))
                    {
                        nearestPickable = pickable;
                    }
                }
            }

            return nearestPickable;
        }

        public PickableController GetNearestTypedPickable(Vector3 selfPosition, PickableCategory type)
        {
            PickableController nearestPickable = null;

            foreach (var pickable in pickablesInSight)
            {
                if (pickable != null)
                {
                    if (pickable.Category == type)
                    {
                        if (nearestPickable == null)
                        {
                            nearestPickable = pickable;
                        }
                        else if (base.IsVectorCloserThanOtherVector(
                            selfPosition,
                            pickable.transform.root.position,
                            nearestPickable.transform.root.position))
                        {
                            nearestPickable = pickable;
                        }
                    }
                }
            }

            return nearestPickable;
        }

        private void OnPickableDestroy(PickableController pickable)
        {
            if (pickable == pickableTarget)
            {
                pickableTarget = null;
            }

            Remove(pickable);
        }
    }
}