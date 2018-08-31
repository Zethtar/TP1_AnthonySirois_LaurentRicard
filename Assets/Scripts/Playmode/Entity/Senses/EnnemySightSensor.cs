using System.Collections.Generic;
using Playmode.Ennemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemySightSensorEventHandler(EnnemyController ennemy);
    
    public class EnnemySightSensor : EnnemySensor
    {
        public event EnnemySightSensorEventHandler OnEnemySeen;
        public event EnnemySightSensorEventHandler OnEnemySightLost;

        public override void TriggerEnter(EnnemyController ennemy)
        {
            See(ennemy);
        }

        public override void TriggerExit(EnnemyController ennemy)
        {
            LooseSightOf(ennemy);
        }

        private void See(EnnemyController ennemy)
        {
            NotifyEnnemySeen(ennemy);
        }

        private void LooseSightOf(EnnemyController ennemy)
        {
            NotifyEnnemySightLost(ennemy);
        }

        private void NotifyEnnemySeen(EnnemyController ennemy)
        {
            OnEnemySeen?.Invoke(ennemy);
        }

        private void NotifyEnnemySightLost(EnnemyController ennemy)
        {
            OnEnemySightLost?.Invoke(ennemy);
        }
    }
}