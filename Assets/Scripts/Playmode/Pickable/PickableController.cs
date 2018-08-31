using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{
    public delegate void PickableDestroyEventHandler(PickableController pickable);

    public abstract class PickableController : MonoBehaviour
    {
        public event PickableDestroyEventHandler OnPickableDestroy;

        protected Destroyer destroyer;
        private EnnemyCollisionSensor enemyCollisionSensor;

        public PickableCategory Category { get; protected set; }

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponents();
        }

        private void OnEnable()
        {
            enemyCollisionSensor.OnCollision += OnCollision;
        }

        private void OnDisable()
        {
            NotifyPickableDestroy(this);
            enemyCollisionSensor.OnCollision -= OnCollision;
        }

        private void InitializeComponents()
        {
            destroyer = GetComponent<RootDestroyer>();
            enemyCollisionSensor = transform.root.GetComponentInChildren<EnnemyCollisionSensor>();
        }

        protected abstract void ValidateSerialisedFields();
        protected abstract void OnCollision(EnnemyController enemy);

        private void NotifyPickableDestroy(PickableController pickable)
        {
            OnPickableDestroy?.Invoke(pickable);
        }
    }
}

