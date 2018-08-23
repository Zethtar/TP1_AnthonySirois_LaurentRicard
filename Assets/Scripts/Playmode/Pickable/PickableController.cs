using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Entity.Destruction;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{	
	public class PickableController : MonoBehaviour 
	{
		[SerializeField] private GameObject body;
		
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
			if (body == null)
				throw new ArgumentException("Body parts must be provided. Body is missing.");
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

		public void Configure(PickableType type)
		{
			switch (type)
			{		
					case PickableType.Shotgun:
						body.GetComponent<SpriteRenderer>().sprite = shotgunSprite;
						break;
					
					case PickableType.Uzi:
						body.GetComponent<SpriteRenderer>().sprite = uziSprite;
						break;
					
					case  PickableType.Invincible:
						//body.GetComponent<SpriteRenderer>().sprite = invicibleSprite;
						//break;
					
					default: //case PickableType.MedicalKit:
						body.GetComponent<SpriteRenderer>().sprite = medicalKitSprite;
						break;
			}
		}
	}
}

