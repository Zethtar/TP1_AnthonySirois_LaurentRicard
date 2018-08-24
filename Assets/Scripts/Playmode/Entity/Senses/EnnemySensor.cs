using System.Collections.Generic;
using Playmode.Ennemy;
using UnityEditor.Compilation;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public abstract class EnnemySensor : MonoBehaviour
    {
        public abstract void TriggerEnter(EnnemyController ennemy);

        public abstract void TriggerExit(EnnemyController ennemy);
    }
}