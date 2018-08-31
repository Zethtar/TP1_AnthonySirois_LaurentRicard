using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;

namespace Playmode.Ennemy.Memory
{
    public class EnnemyEnnemyMemory : EnnemyMemory
    {
        private ICollection<EnnemyController> enemiesInSight;

        public IEnumerable<EnnemyController> EnemiesInSight => enemiesInSight;

        private EnnemyController target;
        
        public EnnemyEnnemyMemory()
        {
            enemiesInSight = new HashSet<EnnemyController>();
        }

        public EnnemyController GetEnemyTarget()
        {
            return target;
        }

        public void Add(EnnemyController enemy)
        {
            enemiesInSight.Add(enemy);
            enemy.OnOtherEnemyDeath += OnOtherEnemyDeath;
        }

        public void Remove(EnnemyController enemy)
        {
            if (target == enemy)
            {
                target = null;
            }

            enemiesInSight.Remove(enemy);
            enemy.OnOtherEnemyDeath -= OnOtherEnemyDeath;
        }

        public bool IsAnEnemyInSight()
        {
            return (enemiesInSight.Count > 0);
        }

        public void TargetNearestEnemy(Vector3 selfPosition)
        {
            EnnemyController nearestEnemy = null;

            foreach (EnnemyController enemy in enemiesInSight)
            {
                if (enemy != null)
                {
                    if (nearestEnemy == null)
                    {
                        nearestEnemy = enemy;
                    }
                    else if (base.IsVectorCloserThanOtherVector(
                        selfPosition,
                        enemy.transform.root.position,
                        nearestEnemy.transform.root.position))
                    {
                        nearestEnemy = enemy;
                    }
                }
            }

            target = nearestEnemy;
        }

        private void OnOtherEnemyDeath(EnnemyController enemy)
        {
            if (target == enemy)
            {
                target = null;
            }

            Remove(enemy);
        }
    }
}