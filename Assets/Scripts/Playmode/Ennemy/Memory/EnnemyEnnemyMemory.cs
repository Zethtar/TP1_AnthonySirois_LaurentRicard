using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;

namespace Playmode.Ennemy.Memory
{
    public class EnnemyEnnemyMemory : EnnemyMemory
    {
        private ICollection<EnnemyController> ennemiesInSight;

        public IEnumerable<EnnemyController> EnnemiesInSight => ennemiesInSight;

        private EnnemyController enemyTarget = null;


        public EnnemyEnnemyMemory()
        {
            ennemiesInSight = new HashSet<EnnemyController>();
        }


        public EnnemyController GetEnnemyTarget()
        {
            return enemyTarget;
        }

        public void AddEnemy(EnnemyController ennemy)
        {
            ennemiesInSight.Add(ennemy);
            ennemy.OnOtherEnnemyDeath += OnOtherEnnemyDeath;
        }

        public void RemoveEnemy(EnnemyController ennemy)
        {
            if (enemyTarget == ennemy)
            {
                enemyTarget = null;
            }
            ennemiesInSight.Remove(ennemy);
            ennemy.OnOtherEnnemyDeath -= OnOtherEnnemyDeath;
        }

        public bool IsAnEnnemyInSight()
        {
            return (ennemiesInSight.Count > 0);
        }

        public EnnemyController GetNearestEnnemy(Vector3 selfPosition)
        {

            EnnemyController nearestEnnemy = null;

            foreach (EnnemyController ennemy in ennemiesInSight)
            {
                if (ennemy != null)
                {
                    if (nearestEnnemy == null)
                    {
                        nearestEnnemy = ennemy;
                    }
                    else if (!base.IsVectorNearestThanOtherVector(
                        selfPosition,
                        nearestEnnemy.transform.root.position,
                        ennemy.transform.root.position))
                    {
                        nearestEnnemy = ennemy;
                    }
                }

            }
            enemyTarget = nearestEnnemy;
            return nearestEnnemy;
        }



        private void OnOtherEnnemyDeath(EnnemyController enemy)
        {
            if (enemy == enemyTarget)
            {
                enemyTarget = null;
            }
            RemoveEnemy(enemy);
        }
    }
}