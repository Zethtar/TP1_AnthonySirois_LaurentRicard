using Playmode.Enemy;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public class EnemyStimulus : MonoBehaviour
    {
        private EnemyController enemy;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            enemy = transform.root.GetComponentInChildren<EnemyController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<EnemySensor>()?.TriggerEnter(enemy);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<EnemySensor>()?.TriggerExit(enemy);
        }
    }
}