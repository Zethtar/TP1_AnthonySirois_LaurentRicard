using Playmode.Enemy;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemySightSensorEventHandler(EnemyController enemy);

    public class EnemySightSensor : EnemySensor
    {
        public event EnnemySightSensorEventHandler OnEnemySeen;
        public event EnnemySightSensorEventHandler OnEnemySightLost;

        public override void TriggerEnter(EnemyController enemy)
        {
            See(enemy);
        }

        public override void TriggerExit(EnemyController enemy)
        {
            LooseSightOf(enemy);
        }

        private void See(EnemyController enemy)
        {
            NotifyEnnemySeen(enemy);
        }

        private void LooseSightOf(EnemyController enemy)
        {
            NotifyEnnemySightLost(enemy);
        }

        private void NotifyEnnemySeen(EnemyController enemy)
        {
            OnEnemySeen?.Invoke(enemy);
        }

        private void NotifyEnnemySightLost(EnemyController enemy)
        {
            OnEnemySightLost?.Invoke(enemy);
        }
    }
}