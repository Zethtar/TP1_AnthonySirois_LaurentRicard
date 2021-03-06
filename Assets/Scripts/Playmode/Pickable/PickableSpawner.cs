﻿using System;
using System.Collections;
using Playmode.Pickable.Types;
using Playmode.Util.Values;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Playmode.Pickable
{
    public class PickableSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject medKitPrefab;
        [SerializeField] private GameObject shotgunPrefab;
        [SerializeField] private float spawnDelayInSeconds = 5f;
        [SerializeField] private GameObject uziPrefab;

        private void Awake()
        {
            ValidateSerialisedFields();

            SpawnPickableOnEverySpawnPoint();
        }

        private void ValidateSerialisedFields()
        {
            if (medKitPrefab == null)
                throw new ArgumentException("Can't spawn null medKit prefab.");
            if (uziPrefab == null)
                throw new ArgumentException("Can't spawn null uzi prefab.");
            if (shotgunPrefab == null)
                throw new ArgumentException("Can't spawn null shotgun prefab.");
            if (transform.childCount <= 0)
                throw new ArgumentException("Can't spawn pickables whitout spawn points. " +
                                            "Create chilldrens for this GameObject as spawn points.");
        }

        private void SpawnPickable(Vector3 position, GameObject prefab)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }

        private void SpawnRandomPickable()
        {
            var position = transform.GetChild(Random.Range(0, transform.childCount - 1)).position;

            GameObject prefab;
            switch ((PickableType) Random.Range(0, (int) PickableType.TotalType))
            {
                case PickableType.Shotgun:
                    prefab = shotgunPrefab;
                    break;

                case PickableType.Uzi:
                    prefab = uziPrefab;
                    break;

                default: //case PickableType.MedicalKit
                    prefab = medKitPrefab;
                    break;
            }

            if (IsSpawnerIsEmpty(position)) SpawnPickable(position, prefab);
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

        private static bool IsSpawnerIsEmpty(Vector3 position)
        {
            var pickables = GameObject.FindGameObjectsWithTag(Tags.Pickable);

            foreach (var currentPickable in pickables)
                if (currentPickable.transform.position == position)
                    return false;

            return true;
        }

        private void SpawnPickableOnEverySpawnPoint()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                GameObject prefab;
                var position = transform.GetChild(i).position;

                switch ((PickableType) Random.Range(0, (int) PickableType.TotalType))
                {
                    case PickableType.Shotgun:
                        prefab = shotgunPrefab;
                        break;

                    case PickableType.Uzi:
                        prefab = uziPrefab;
                        break;

                    default: //case PickableType.MedicalKit
                        prefab = medKitPrefab;
                        break;
                }

                SpawnPickable(position, prefab);
            }
        }
    }
}