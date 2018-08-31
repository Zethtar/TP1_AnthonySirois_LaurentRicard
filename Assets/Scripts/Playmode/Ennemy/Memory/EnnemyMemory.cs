using UnityEngine;
using System.Collections;

namespace Playmode.Ennemy.Memory
{
    public abstract class EnnemyMemory
    {
        protected EnnemyMemory()
        {

        }

        protected bool IsVectorCloserThanOtherVector(Vector3 selfPosition, Vector3 firstPosition, Vector3 secondPosition)
        {
            return ((Vector3.Distance(selfPosition, firstPosition)) <
                (Vector3.Distance(selfPosition, secondPosition)));
        }
    }
}