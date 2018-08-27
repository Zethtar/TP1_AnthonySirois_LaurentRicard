﻿using System;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Ennemy
{
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

        private Health health;
        private Mover mover;
        private Destroyer destroyer;
        private EnnemySightSensor ennemySightSensor;
        private HitSensor hitSensor;
        private HandController handController;

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
            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();
            handController = hand.GetComponent<HandController>();

            strategy = new TurnAndShootStragegy(mover, handController);
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
            hitSensor.OnHit -= OnHit;
            health.OnDeath -= OnDeath;
        }

        public void Configure(EnnemyStrategy strategy, Color color)
        {
            body.GetComponent<SpriteRenderer>().color = color;
            sight.GetComponent<SpriteRenderer>().color = color;
            
            switch (strategy)
            {
                case EnnemyStrategy.Careful:
                    typeSign.GetComponent<SpriteRenderer>().sprite = carefulSprite;
                    break;
                case EnnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    break;
                case EnnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    break;
                default:
                    typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
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
            Debug.Log("R.I.P.");

            destroyer.Destroy();
        }

        private void OnEnnemySeen(EnnemyController ennemy)
        {
            Debug.Log("Enemy in sight");
        }

        private void OnEnnemySightLost(EnnemyController ennemy)
        {
            Debug.Log("Enemy out of sight");
        }

        private void OnPickup(PickableType type)
        {
            Debug.Log("I've picked up something");
        }

        public void Heal(int hitPoints)
        {
            Debug.Log("I feel better");
            
            health.Heal(hitPoints);
        }

        public void Equip(GameObject weapon)
        {
            Debug.Log("I equipe a new weapon");
            
            handController.Hold(weapon);
        }
    }
}