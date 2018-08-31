using System;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Memory;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Ennemy
{
    public delegate void EnnemyDeathEventHandler(EnnemyController enemy);

    public class EnnemyController : MonoBehaviour
    {
        [Header("Body Parts")] [SerializeField]
        private GameObject body;

        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject sight;
        [SerializeField] private GameObject typeSign;

        [Header("Type Images")] [SerializeField]
        private Sprite normalSprite;

        [SerializeField] private Sprite carefulSprite;
        [SerializeField] private Sprite cowboySprite;
        [SerializeField] private Sprite camperSprite;
        [Header("Behaviour")] [SerializeField] private GameObject startingWeaponPrefab;

        public event EnnemyDeathEventHandler OnOtherEnemyDeath;

        private Health health;
        private Mover mover;
        private Destroyer destroyer;
        private EnnemySightSensor enemySightSensor;
        private PickableSightSensor pickableSightSensor;
        private HitSensor hitSensor;
        private HandController handController;

        private EnnemyEnnemyMemory enemyMemory;
        private EnnemyPickableMemory pickableMemory;
        private IEnnemyStrategy strategy;

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
            enemySightSensor = rootTransform.GetComponentInChildren<EnnemySightSensor>();
            pickableSightSensor = rootTransform.GetComponentInChildren<PickableSightSensor>();
            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            handController = hand.GetComponent<HandController>();
            pickableMemory = new EnnemyPickableMemory();
            enemyMemory = new EnnemyEnnemyMemory();
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
            pickableMemory = new EnnemyPickableMemory();
            enemyMemory = new EnnemyEnnemyMemory();

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

        private void OnEnemySeen(EnnemyController enemy)
        {
            enemyMemory.Add(enemy);
        }

        private void OnEnemySightLost(EnnemyController enemy)
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

        private void NotifyOtherEnemyDeath(EnnemyController enemy)
        {
            if (OnOtherEnemyDeath != null) OnOtherEnemyDeath(enemy);
        }
    }
}