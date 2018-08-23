using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Pickable
{
	public class PickableSpawner : MonoBehaviour 
	{
		[SerializeField] private GameObject pickablePrefab;

		private void Awake()
		{
			ValidateSerialisedFields();
		}
		
		private void ValidateSerialisedFields()
		{
			if (pickablePrefab == null)
				throw new ArgumentException("Can't spawn null pickable prefab.");
			if (transform.childCount <= 0)
				throw new ArgumentException("Can't spawn pickables whitout spawn points. " +
				                            "Create chilldrens for this GameObject as spawn points.");
		}

		private void SpawnPickable(Vector3 position)
		{
			Instantiate(pickablePrefab, position, Quaternion.identity);
			//.GetComponentInChildren<EnnemyController>()
			//	.Configure(strategy, color);
		}
	}
}


