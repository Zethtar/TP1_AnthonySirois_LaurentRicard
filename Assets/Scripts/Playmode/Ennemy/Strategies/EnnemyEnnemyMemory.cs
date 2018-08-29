using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;

public class EnnemyEnnemyMemory : MonoBehaviour
{
    private ICollection<EnnemyController> ennemiesInSight;

    public IEnumerable<EnnemyController> EnnemiesInSight => ennemiesInSight;

    public EnnemyEnnemyMemory()
    {
        ennemiesInSight = new HashSet<EnnemyController>();
    }

    public void See(EnnemyController ennemy)
    {
        ennemiesInSight.Add(ennemy);
        ennemy.OnOtherEnnemyDeath += OnOtherEnnemyDeath;    
    }

    public void LooseSightOf(EnnemyController ennemy)
    {
        ennemiesInSight.Remove(ennemy);
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
            if(nearestEnnemy == null)
            {
                nearestEnnemy = ennemy;
            }
            else if (IsEnnemyNearestThanOtherEnnemy(selfPosition, nearestEnnemy, ennemy))
            {
                nearestEnnemy = ennemy;
            }
        }
        return nearestEnnemy;
    }

    private bool IsEnnemyNearestThanOtherEnnemy (Vector3 selfPosition, EnnemyController firstEnnemy, EnnemyController secondEnnemy)
    {
        return ((Vector3.Distance(selfPosition, firstEnnemy.transform.position)) < 
            (Vector3.Distance(selfPosition, secondEnnemy.transform.position)));
    }

    private void OnOtherEnnemyDeath(EnnemyController ennemy)
    {
        LooseSightOf(ennemy);
    }
}
