using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class TurnAndShootStragegy : IEnnemyStrategy
    {
        private readonly Mover mover;
        private readonly HandController handController;

        private WeaponController weaponTarget;
        private EnnemyController target;
        private EnnemyState state;

        public TurnAndShootStragegy(Mover mover, HandController handController)
        {
            this.mover = mover;
            this.handController = handController;
        }

        public void Act()
        {
            mover.Rotate(Mover.Clockwise);

            handController.Use();
        }

        public void SetEnnemyTarget(EnnemyController target)
        {
            this.target = target;
        }

        public void SetWeaponTarget(WeaponController weaponTarget)
        {
            this.weaponTarget = weaponTarget;
        }

        public void SetState(EnnemyState state)
        {
            this.state = state;
        }
    }
}