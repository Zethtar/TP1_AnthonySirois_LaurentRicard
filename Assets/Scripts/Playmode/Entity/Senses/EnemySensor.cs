using Playmode.Enemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public abstract class EnemySensor : MonoBehaviour
    {
        public abstract void TriggerEnter(EnemyController enemy);

        public abstract void TriggerExit(EnemyController enemy);
    }
}