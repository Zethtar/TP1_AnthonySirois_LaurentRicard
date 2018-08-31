using System;
using Playmode.Enemy.BodyParts;
using Playmode.Enemy.Memory;
using Playmode.Enemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable;
using UnityEngine;

namespace Playmode.Enemy
{
    public delegate void EnnemyDeathEventHandler(EnemyController enemy);

    public class EnemyController : MonoBehaviour
    {
        [Header("Body Parts")] [SerializeField]
        private GameObject body;

        [SerializeField] private Sprite camperSprite;

        [SerializeField] private Sprite carefulSprite;
        [SerializeField] private Sprite cowboySprite;
        private Destroyer destroyer;

        private EnemyEnemyMemory enemyMemory;
        private EnemySightSensor enemySightSensor;

        [SerializeField] private GameObject hand;
        private HandController handController;

        private Health health;
        private HitSensor hitSensor;
        private Mover mover;

        [Header("Type Images")] [SerializeField]
        private Sprite normalSprite;

        private EnemyPickableMemory pickableMemory;
        private PickableSightSensor pickableSightSensor;
        [SerializeField] private GameObject sight;
        [Header("Behaviour")] [SerializeField] private GameObject startingWeaponPrefab;
        private IEnnemyStrategy strategy;
        [SerializeField] private GameObject typeSign;

        public event EnnemyDeathEventHandler OnOtherEnemyDeath;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
            CreateStartingWeapon();
        }

        private void ValidateSerialisedFields()
        {
            if (body == null)
                throw new ArgumentException("Body parts must be provided. Body is missing.");
            if (hand == null)
                throw new ArgumentException("Body parts must be provided. Hand is missing.");
            if (sight == null)
                throw new ArgumentException("Body parts must be provided. Sight is missing.");
            if (typeSign == null)
                throw new ArgumentException("Body parts must be provided. TypeSign is missing.");
            if (normalSprite == null)
                throw new ArgumentException("Type sprites must be provided. Normal is missing.");
            if (carefulSprite == null)
                throw new ArgumentException("Type sprites must be provided. Careful is missing.");
            if (cowboySprite == null)
                throw new ArgumentException("Type sprites must be provided. Cowboy is missing.");
            if (camperSprite == null)
                throw new ArgumentException("Type sprites must be provided. Camper is missing.");
            if (startingWeaponPrefab == null)
                throw new ArgumentException("StartingWeapon prefab must be provided.");
        }

        private void InitializeComponent()
        {
            health = GetComponent<Health>();
            mover = GetComponent<RootMover>();
            destroyer = GetComponent<RootDestroyer>();

            var rootTransform = transform.root;
            enemySightSensor = rootTransform.GetComponentInChildren<EnemySightSensor>();
            pickableSightSensor = rootTransform.GetComponentInChildren<PickableSightSensor>();
            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            handController = hand.GetComponent<HandController>();
            pickableMemory = new EnemyPickableMemory();
            enemyMemory = new EnemyEnemyMemory();
            strategy = new NormalStrategy(mover, handController, enemyMemory, pickableMemory);
        }

        private void CreateStartingWeapon()
        {
            handController.Hold(Instantiate(
                startingWeaponPrefab,
                Vector3.zero,
                Quaternion.identity
            ));
        }

        private void OnEnable()
        {
            enemySightSensor.OnEnemySeen += OnEnemySeen;
            enemySightSensor.OnEnemySightLost += OnEnemySightLost;
            pickableSightSensor.OnPickableSeen += OnPickableSeen;
            pickableSightSensor.OnPickableSightLost += OnPickableSightLost;
            hitSensor.OnHit += OnHit;
            health.OnDeath += OnDeath;
        }

        private void Update()
        {
            strategy.Act();
        }

        private void OnDisable()
        {
            enemySightSensor.OnEnemySeen -= OnEnemySeen;
            enemySightSensor.OnEnemySightLost -= OnEnemySightLost;
            pickableSightSensor.OnPickableSeen -= OnPickableSeen;
            pickableSightSensor.OnPickableSightLost -= OnPickableSightLost;
            hitSensor.OnHit -= OnHit;
            health.OnDeath -= OnDeath;
        }

        public void Configure(EnemyStrategy newStrategy, Color color)
        {
            body.GetComponent<SpriteRenderer>().color = color;
            sight.GetComponent<SpriteRenderer>().color = color;
            pickableMemory = new EnemyPickableMemory();
            enemyMemory = new EnemyEnemyMemory();

            switch (newStrategy)
            {
                case EnemyStrategy.Careful:
                    typeSign.GetComponent<SpriteRenderer>().sprite = carefulSprite;
                    strategy = new CarefulStrategy(mover, handController, health, enemyMemory,
                        pickableMemory);
                    break;
                case EnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    strategy = new CowboyStrategy(mover, handController, enemyMemory, pickableMemory);
                    break;
                case EnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    strategy = new CamperStrategy(mover, handController, health, enemyMemory,
                        pickableMemory);
                    break;
                default:
                    typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
                    strategy = new NormalStrategy(mover, handController, enemyMemory, pickableMemory);
                    health.Heal(50);
                    break;
            }
        }

        private void OnHit(int hitPoints)
        {
            health.Hit(hitPoints);
        }

        private void OnDeath()
        {
            NotifyOtherEnemyDeath(this);
            destroyer.Destroy();
        }

        private void OnEnemySeen(EnemyController enemy)
        {
            enemyMemory.Add(enemy);
        }

        private void OnEnemySightLost(EnemyController enemy)
        {
            enemyMemory.Remove(enemy);
        }

        private void OnPickableSeen(PickableController pickable)
        {
            pickableMemory.Add(pickable);
        }

        private void OnPickableSightLost(PickableController pickable)
        {
            pickableMemory.Remove(pickable);
        }

        public void Heal(int hitPoints)
        {
            health.Heal(hitPoints);
        }

        public void Equip(GameObject weapon)
        {
            handController.Hold(weapon);
        }

        private void NotifyOtherEnemyDeath(EnemyController enemy)
        {
            if (OnOtherEnemyDeath != null) OnOtherEnemyDeath(enemy);
        }
    }
}