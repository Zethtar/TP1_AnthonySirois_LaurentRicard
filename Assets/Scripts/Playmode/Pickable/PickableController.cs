using Playmode.Enemy;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{
    public delegate void PickableDestroyEventHandler(PickableController pickable);

    public abstract class PickableController : MonoBehaviour
    {
        protected Destroyer destroyer;
        private EnemyCollisionSensor enemyCollisionSensor;

        public PickableCategory Category { get; protected set; }
        public event PickableDestroyEventHandler OnPickableDestroy;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponents();
        }

        private void OnEnable()
        {
            enemyCollisionSensor.OnCollision += OnCollision;
        }

        private void OnDisable()
        {
            NotifyPickableDestroy(this);
            enemyCollisionSensor.OnCollision -= OnCollision;
        }

        private void InitializeComponents()
        {
            destroyer = GetComponent<RootDestroyer>();
            enemyCollisionSensor = transform.root.GetComponentInChildren<EnemyCollisionSensor>();
        }

        protected abstract void ValidateSerialisedFields();
        protected abstract void OnCollision(EnemyController enemy);

        private void NotifyPickableDestroy(PickableController pickable)
        {
            OnPickableDestroy?.Invoke(pickable);
        }
    }
}