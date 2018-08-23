using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Pickable.Types;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Playmode.Pickable
{
	public class PickableSpawner : MonoBehaviour 
	{
		[SerializeField] private GameObject pickablePrefab;
		[SerializeField] private float spawnDelayInSeconds = 5f;

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

		private void SpawnPickable(Vector3 position, PickableType type)
		{
			Debug.Log("Pickable spawned");
			
			Instantiate(pickablePrefab, position, Quaternion.identity)
				.GetComponentInChildren<PickableController>()
				.Configure(type);
		}

		private void SpawnRandomPickable()
		{
			var position = transform.GetChild(Random.Range(0, transform.childCount - 1)).position;
			var type = (PickableType)Random.Range(0, (int)PickableType.TotalType);

			SpawnPickable(position, type);
		}
		
		private IEnumerator SpawnPickableRoutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(spawnDelayInSeconds);
				SpawnRandomPickable();
			}
		}
		
		private void OnEnable()
		{
			StartCoroutine(SpawnPickableRoutine());		
		}
	
		private void OnDisable()
		{
			StopAllCoroutines();
		}
	}
}

