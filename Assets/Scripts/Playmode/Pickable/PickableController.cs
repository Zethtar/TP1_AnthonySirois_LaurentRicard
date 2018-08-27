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
	public abstract class PickableController : MonoBehaviour 
	{		
		protected Destroyer destroyer;
		private EnnemyCollisionSensor ennemyCollisionSensor;

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
			ennemyCollisionSensor.OnCollision -= OnCollision;
		}

		private void ValidateSerialisedFields()
		{
		}

		private void InitializeComponents()
		{
			destroyer = GetComponent<RootDestroyer>();
			ennemyCollisionSensor = transform.root.GetComponentInChildren<EnnemyCollisionSensor>();
		}

		protected abstract void OnCollision(EnnemyController ennemy);
	}
}

