using UnityEngine;

namespace Playmode.Movement
{
    public class AnchoredMover : Mover
    {
        private Transform rootTransform;

        private new void Awake()
        {
            base.Awake();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            rootTransform = transform.root;
        }

        public override void Move(Vector3 direction)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }

        public override void MoveToTarget(Vector3 target)
        {
            transform.Translate((target - transform.root.position).normalized * speed * Time.deltaTime);
        }

        public override void Rotate(float direction)
        {
            transform.RotateAround(
                rootTransform.position,
                Vector3.forward,
                (direction < 0 ? rotateSpeed : -rotateSpeed) * Time.deltaTime
            );
        }

        public override void RotateToTarget(Vector3 target)
        {

        }
    }
}