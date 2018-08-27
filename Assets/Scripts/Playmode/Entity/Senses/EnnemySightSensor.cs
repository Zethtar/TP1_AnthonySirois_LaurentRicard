using System.Collections.Generic;
using Playmode.Ennemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void EnnemySightSensorEventHandler(EnnemyController ennemy);
    
    public class EnnemySightSensor : EnnemySensor
    {
        private ICollection<EnnemyController> ennemiesInSight;

        public event EnnemySightSensorEventHandler OnEnnemySeen;
        public event EnnemySightSensorEventHandler OnEnnemySightLost;

        public IEnumerable<EnnemyController> EnnemiesInSight => ennemiesInSight;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ennemiesInSight = new HashSet<EnnemyController>();
        }

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
            ennemiesInSight.Add(ennemy);

            NotifyEnnemySeen(ennemy);
        }

        private void LooseSightOf(EnnemyController ennemy)
        {
            ennemiesInSight.Remove(ennemy);

            NotifyEnnemySightLost(ennemy);
        }

        private void NotifyEnnemySeen(EnnemyController ennemy)
        {
            if (OnEnnemySeen != null) OnEnnemySeen(ennemy);
        }

        private void NotifyEnnemySightLost(EnnemyController ennemy)
        {
            if (OnEnnemySightLost != null) OnEnnemySightLost(ennemy);
        }
    }
}