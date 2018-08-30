using System;
using Playmode.Ennemy.BodyParts;
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

        private EnnemyEnnemyMemory ennemyEnnemyMemory;
        private EnnemyPickableMemory ennemyPickableMemory;
        private EnnemyState state;
        private Vector3 target;

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
            ennemyPickableMemory = new EnnemyPickableMemory();
            ennemyEnnemyMemory = new EnnemyEnnemyMemory();
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
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    strategy = new CarefulStrategy(mover, handController, health, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                case EnnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    strategy = new CowboyStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                case EnnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    strategy = new CamperStrategy(mover, handController, health, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
                default:
                    typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
                    ennemyPickableMemory = new EnnemyPickableMemory();
                    ennemyEnnemyMemory = new EnnemyEnnemyMemory();
                    strategy = new NormalStrategy(mover, handController, ennemyEnnemyMemory, ennemyPickableMemory);
                    strategy.SetState(EnnemyState.Roaming);
                    break;
            }

            ennemyPickableMemory = new EnnemyPickableMemory();
            ennemyEnnemyMemory = new EnnemyEnnemyMemory();
            strategy = new CarefulStrategy(mover, handController, health, ennemyEnnemyMemory, ennemyPickableMemory);
            strategy.SetState(EnnemyState.Roaming);//TODO Delete this
        }

        private void OnHit(int hitPoints)
        {
            health.Hit(hitPoints);
        }

        private void OnDeath()
        {
            NotifyOtherEnnemyDeath(this);
            destroyer.Destroy();
        }

        private void OnEnnemySeen(EnnemyController ennemy)
        {
            Debug.Log("Enemy in sight");
            strategy.SetEnnemyTarget(ennemy);
        }

        private void OnEnnemySightLost(EnnemyController ennemy)
        {
            Debug.Log("Enemy out of sight");
        }

        private void OnPickableSeen(PickableController pickable)
        {
            Debug.Log("Item seen");
        }

        private void OnPickableSightLost(PickableController pickable)
        {
            Debug.Log("Item lost");
        }

        public void Heal(int hitPoints)
        {   
            health.Heal(hitPoints);
        }

        public void Equip(GameObject weapon)
        { 
            handController.Hold(weapon);
        }

        private void NotifyOtherEnnemyDeath(EnnemyController ennemy)
        {
            if (OnOtherEnnemyDeath != null) OnOtherEnnemyDeath(ennemy);
        }
    }
}