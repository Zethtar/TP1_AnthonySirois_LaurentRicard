using UnityEngine;

namespace Playmode.Enemy.Memory
{
    public abstract class EnemyMemory
    {
        protected bool IsVectorCloserThanOtherVector(Vector3 selfPosition, Vector3 firstPosition,
            Vector3 secondPosition)
        {
            return Vector3.Distance(selfPosition, firstPosition) <
                   Vector3.Distance(selfPosition, secondPosition);
        }
    }
}