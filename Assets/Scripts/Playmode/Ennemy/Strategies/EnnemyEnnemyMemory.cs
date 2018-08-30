using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;

public class EnnemyEnnemyMemory
{
    private ICollection<EnnemyController> ennemiesInSight;

    public IEnumerable<EnnemyController> EnnemiesInSight => ennemiesInSight;

    public EnnemyEnnemyMemory()
    {
        ennemiesInSight = new HashSet<EnnemyController>();
    }

    public void AddEnemy(EnnemyController ennemy)
    {
        ennemiesInSight.Add(ennemy);
        ennemy.OnOtherEnnemyDeath += OnOtherEnnemyDeath;
    }

    public void RemoveEnemy(EnnemyController ennemy)
    {
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
                if(nearestEnnemy == null)
                {
                    nearestEnnemy = ennemy;
                }
                else if (IsEnnemyNearestThanOtherEnnemy(
                    selfPosition,
                    nearestEnnemy.transform.root.position,
                    ennemy.transform.root.position))
                {
                    nearestEnnemy = ennemy;
                }
            }
            
        }
        return nearestEnnemy;
    }

    private bool IsEnnemyNearestThanOtherEnnemy (Vector3 selfPosition, Vector3 firstEnnemyPosition, Vector3 secondEnnemyPosition)
    {
        return ((Vector3.Distance(selfPosition, firstEnnemyPosition)) < 
            (Vector3.Distance(selfPosition, secondEnnemyPosition)));
    }

    private void OnOtherEnnemyDeath(EnnemyController ennemy)
    {
        RemoveEnemy(ennemy);
    }
}
