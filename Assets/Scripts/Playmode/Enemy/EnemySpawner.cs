using System;
using Playmode.Enemy.Strategies;
using Playmode.Util.Collections;
using UnityEngine;

namespace Playmode.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private static readonly Color[] DefaultColors =
        {
            Color.white, Color.black, Color.blue, Color.cyan, Color.green,
            Color.magenta, Color.red, Color.yellow, new Color(255, 125, 0, 255),
            Color.grey, new Color(125, 255, 46, 255), new Color(125, 0, 125)
        };

        private static readonly EnemyStrategy[] DefaultStrategies =
        {
            EnemyStrategy.Normal,
            EnemyStrategy.Careful,
            EnemyStrategy.Cowboy,
            EnemyStrategy.Camper
        };

        [SerializeField] private Color[] colors = DefaultColors;

        [SerializeField] private GameObject ennemyPrefab;

        private void Awake()
        {
            ValidateSerialisedFields();
        }

        private void Start()
        {
            SpawnEnnemies();
        }

        private void ValidateSerialisedFields()
        {
            if (ennemyPrefab == null)
                throw new ArgumentException("Can't spawn null ennemy prefab.");
            if (colors == null || colors.Length == 0)
                throw new ArgumentException("Ennemies needs colors to be spawned.");
            if (transform.childCount <= 0)
                throw new ArgumentException("Can't spawn ennemis whitout spawn points. " +
                                            "Create chilldrens for this GameObject as spawn points.");
        }

        private void SpawnEnnemies()
        {
            var stragegyProvider = new LoopingEnumerator<EnemyStrategy>(DefaultStrategies);
            var colorProvider = new LoopingEnumerator<Color>(colors);

            for (var i = 0; i < transform.childCount; i++)
                SpawnEnnemy(
                    transform.GetChild(i).position,
                    stragegyProvider.Next(),
                    colorProvider.Next()
                );
        }

        private void SpawnEnnemy(Vector3 position, EnemyStrategy strategy, Color color)
        {
            var ennemy = Instantiate(ennemyPrefab, position, Quaternion.identity);
            ennemy.GetComponentInChildren<EnemyController>()
                .Configure(strategy, color);

            var angle = Vector3.Angle(Vector3.up, Vector3.zero - position);
            angle = position.x > 0 ? angle : 360 - angle;
            ennemy.transform.Rotate(Vector3.forward, angle);
        }
    }
}