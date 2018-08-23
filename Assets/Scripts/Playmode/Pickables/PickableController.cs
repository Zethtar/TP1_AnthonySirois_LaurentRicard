using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Entity.Destruction;
using UnityEngine;

namespace Playmode.Pickable
{	
	public class PickableController : MonoBehaviour 
	{
		[Header("Type Images")] [SerializeField] private Sprite medicalKitSprite;
		[SerializeField] private Sprite shotgunSprite;
		[SerializeField] private Sprite uziSprite;
		[SerializeField] private Sprite invicibleSprite;
		
		private Destroyer destroyer;

		private void Awake()
		{
			ValidateSerialisedFields();
			InitializeComponents();
		}

		private void ValidateSerialisedFields()
		{
			if (medicalKitSprite == null)
				throw new ArgumentException("Type sprites must be provided. Medical Kit is missing.");
			if (shotgunSprite == null)
				throw new ArgumentException("Type sprites must be provided. Shotgun is missing.");
			if (uziSprite == null)
				throw new ArgumentException("Type sprites must be provided. Uzi is missing.");
			if (invicibleSprite == null)
				throw new ArgumentException("Type sprites must be provided. Invincible is missing.");
		}

		private void InitializeComponents()
		{
			destroyer = GetComponent<RootDestroyer>();
		}
		
	}
}

