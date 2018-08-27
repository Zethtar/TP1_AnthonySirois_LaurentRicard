using UnityEngine;

namespace Playmode.Movement
{
    public class RootMover : Mover
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
            rootTransform.Translate(direction.normalized * speed * Time.deltaTime);
        }

        public override void MoveToTarget(Vector3 target)
        {
            rootTransform.Translate((target - transform.root.position).normalized * speed * Time.deltaTime, Space.World);
        }

        public override void Rotate(float direction)
        {
            rootTransform.Rotate(
                Vector3.forward,
                (direction < 0 ? rotateSpeed : -rotateSpeed) * Time.deltaTime
            );
        }

        public override void RotateToTarget(Vector3 target)
        {
            Vector3 targetDir = target - transform.position;
            float angle = Vector3.SignedAngle(targetDir, transform.up, transform.forward);

            if (angle < -1 || angle > 1)
            {
                Rotate(angle);
            }
        }
    }
}