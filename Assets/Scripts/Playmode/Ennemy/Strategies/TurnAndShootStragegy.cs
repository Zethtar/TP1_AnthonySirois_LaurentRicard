using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class TurnAndShootStragegy : Strategy
    {

        public TurnAndShootStragegy(Mover mover, HandController handController) : base(mover, handController)
        {
        }

        override public void Act()
        {
            mover.Rotate(Mover.Clockwise);

            handController.Use();
        }
    }
}