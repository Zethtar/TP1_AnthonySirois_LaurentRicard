﻿ using Playmode.Ennemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class EnnemyStimulus : MonoBehaviour
    {
        private EnnemyController ennemy;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ennemy = transform.root.GetComponentInChildren<EnnemyController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<EnnemySensor>()?.TriggerEnter(ennemy);    
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<EnnemySensor>()?.TriggerExit(ennemy);
        }
    }
}