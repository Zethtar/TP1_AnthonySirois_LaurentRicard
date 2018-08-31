﻿using Playmode.Ennemy;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemyCollisionSensorEventHandler(EnnemyController ennemy);
    
    public class EnnemyCollisionSensor : EnnemySensor
    {
        public event EnnemyCollisionSensorEventHandler OnCollision;

        public override void TriggerEnter(EnnemyController ennemy)
        {
            Collision(ennemy);
        }

        public override void TriggerExit(EnnemyController ennemy)
        {
            //Nothing to do
        }

        private void Collision(EnnemyController ennemy)
        {
            NotifyCollision(ennemy);
        }

        private void NotifyCollision(EnnemyController ennemy)
        {
            OnCollision?.Invoke(ennemy);
        }
    }
}