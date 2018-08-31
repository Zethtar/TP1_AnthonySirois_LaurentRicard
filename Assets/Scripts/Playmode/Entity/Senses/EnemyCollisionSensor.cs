using Playmode.Enemy;

namespace Playmode.Entity.Senses
{
    public delegate void EnemyCollisionSensorEventHandler(EnemyController enemy);

    public class EnemyCollisionSensor : EnemySensor
    {
        public event EnemyCollisionSensorEventHandler OnCollision;

        public override void TriggerEnter(EnemyController enemy)
        {
            Collision(enemy);
        }

        public override void TriggerExit(EnemyController enemy)
        {
            //Nothing to do
        }

        private void Collision(EnemyController enemy)
        {
            NotifyCollision(enemy);
        }

        private void NotifyCollision(EnemyController enemy)
        {
            OnCollision?.Invoke(enemy);
        }
    }
}