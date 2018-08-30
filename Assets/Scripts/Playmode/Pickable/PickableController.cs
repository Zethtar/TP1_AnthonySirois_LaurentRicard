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
        private EnnemyCollisionSensor ennemyCollisionSensor;

        public PickableCategory Category { get; protected set; }

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponents();
        }

        private void OnEnable()
        {
            ennemyCollisionSensor.OnCollision += OnCollision;
        }

        private void OnDisable()
        {
            NotifyPickableDestroy(this);
            ennemyCollisionSensor.OnCollision -= OnCollision;
        }

        private void InitializeComponents()
        {
            destroyer = GetComponent<RootDestroyer>();
            ennemyCollisionSensor = transform.root.GetComponentInChildren<EnnemyCollisionSensor>();
        }

        protected abstract void ValidateSerialisedFields();
        protected abstract void OnCollision(EnnemyController ennemy);

        private void NotifyPickableDestroy(PickableController pickable)
        {
            if (OnPickableDestroy != null) OnPickableDestroy(pickable);
        }
    }
}

