using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Enemy.Memory
{
    public class EnemyEnemyMemory : EnemyMemory
    {
        private readonly ICollection<EnemyController> enemiesInSight;

        private EnemyController target;

        public EnemyEnemyMemory()
        {
            enemiesInSight = new HashSet<EnemyController>();
        }

        public IEnumerable<EnemyController> EnemiesInSight => enemiesInSight;

        public EnemyController GetEnemyTarget()
        {
            return target;
        }

        public void Add(EnemyController enemy)
        {
            enemiesInSight.Add(enemy);
            enemy.OnOtherEnemyDeath += OnOtherEnemyDeath;
        }

        public void Remove(EnemyController enemy)
        {
            if (target == enemy) target = null;

            enemiesInSight.Remove(enemy);
            enemy.OnOtherEnemyDeath -= OnOtherEnemyDeath;
        }

        public bool IsAnEnemyInSight()
        {
            return enemiesInSight.Count > 0;
        }

        public void TargetNearestEnemy(Vector3 selfPosition)
        {
            EnemyController nearestEnemy = null;

            foreach (var enemy in enemiesInSight)
                if (enemy != null)
                {
                    if (nearestEnemy == null)
                        nearestEnemy = enemy;
                    else if (IsVectorCloserThanOtherVector(
                        selfPosition,
                        enemy.transform.root.position,
                        nearestEnemy.transform.root.position))
                        nearestEnemy = enemy;
                }

            target = nearestEnemy;
        }

        private void OnOtherEnemyDeath(EnemyController enemy)
        {
            if (target == enemy) target = null;

            Remove(enemy);
        }
    }
}