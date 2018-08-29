using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable;
using System;
using UnityEngine;

namespace Playmode.Ennemy
{
    public delegate void EnnemyDeathEventHandler(EnnemyController ennemy);

    public class EnnemyController : MonoBehaviour
    {
        [Header("Body Parts")] [SerializeField] private GameObject body;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject sight;
        [SerializeField] private GameObject typeSign;
        [Header("Type Images")] [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite carefulSprite;
        [SerializeField] private Sprite cowboySprite;
        [SerializeField] private Sprite camperSprite;
        [Header("Behaviour")] [SerializeField] private GameObject startingWeaponPrefab;

        public event EnnemyDeathEventHandler OnOtherEnnemyDeath;

        private Health health;
        private Mover mover;
        private Destroyer destroyer;
        private EnnemySightSensor ennemySightSensor;
        private PickableSightSensor pickableSightSensor;
        private HitSensor hitSensor;
        private HandController handController;

        private EnnemyState state;
        private Vector3 target;

        private EnnemyEnnemyMemory ennemyEnnemyMemory;
        private EnnemyPickableMemory ennemyPickableMemory;
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
            ennemySightSensor = rootTransform.GetComponentInChildren<EnnemySightSensor>();
            pickableSightSensor = rootTransform.GetComponentInChildren<PickableSightSensor>();
            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            handController = hand.GetComponent<HandController>();
            ennemyEnnemyMemory = new EnnemyEnnemyMemory();
            ennemyPickableMemory = new EnnemyPickableMemory();
            strategy = new NormalStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
            strategy.SetState(EnnemyState.Roaming);
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
            ennemySightSensor.OnEnnemySeen += OnEnnemySeen;
            ennemySightSensor.OnEnnemySightLost += OnEnnemySightLost;
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
            ennemySightSensor.OnEnnemySeen -= OnEnnemySeen;
            ennemySightSensor.OnEnnemySightLost -= OnEnnemySightLost;
            pickableSightSensor.OnPickableSeen -= OnPickableSeen;
            pickableSightSensor.OnPickableSightLost -= OnPickableSightLost;
            hitSensor.OnHit -= OnHit;
            health.OnDeath -= OnDeath;
        }

        public void Configure(EnnemyStrategy newStrategy, Color color)
        {
            body.GetComponent<SpriteRenderer>().color = color;
            sight.GetComponent<SpriteRenderer>().color = color;
            
            switch (newStrategy)
            {
                case EnnemyStrategy.Careful:
                    typeSign.GetComponent<SpriteRenderer>().sprite = carefulSprite;
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    strategy = new CarfulStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                case EnnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    strategy = new CowboyStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                case EnnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    strategy = new CamperStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                default:
                    typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    strategy = new NormalStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
            }
        }

        private void OnHit(int hitPoints)
        {
            Debug.Log("I've been hit");

            health.Hit(hitPoints);
        }

        private void OnDeath()
        {
            Debug.Log("Dead");

            NotifyEnnemyDeath(this);

            destroyer.Destroy();
        }

        private void OnEnnemySeen(EnnemyController ennemy)
        {
            Debug.Log("Enemy in sight");
            ennemyEnnemyMemory.See(ennemy);
        }

        private void OnEnnemySightLost(EnnemyController ennemy)
        {
            Debug.Log("Enemy out of sight");
            ennemyEnnemyMemory.LooseSightOf(ennemy);
        }

        private void OnPickableSeen(PickableController pickable)
        {
            Debug.Log("Item seen");
            ennemyPickableMemory.See(pickable);
        }

        private void OnPickableSightLost(PickableController pickable)
        {
            Debug.Log("Item lost");
            ennemyPickableMemory.LooseSightOf(pickable);
        }

        public void Heal(int hitPoints)
        {
            Debug.Log("Healed");
            
            health.Heal(hitPoints);
        }

        public void Equip(GameObject weapon)
        {
            Debug.Log("New weapon");
            
            handController.Hold(weapon);
        }

        private void NotifyEnnemyDeath(EnnemyController ennemy)
        {
            if (OnOtherEnnemyDeath != null) OnOtherEnnemyDeath(ennemy);
        }
    }
}