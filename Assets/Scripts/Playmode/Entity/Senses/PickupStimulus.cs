using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class PickupStimulus : MonoBehaviour 
    {
        private void Awake()
        {
            ValidateSerializeFields();
        }

        private void ValidateSerializeFields()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //other.GetComponent<Entity.Senses.HitSensor>()?.Hit(hitPoints);
        }
    }
}


