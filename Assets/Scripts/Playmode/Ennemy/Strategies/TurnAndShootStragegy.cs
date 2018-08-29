using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.Strategies
{
    public class TurnAndShootStragegy : Strategy
    {

        public TurnAndShootStragegy(
            Mover mover,
            HandController handController,
            EnnemyEnnemyMemory ennemyEnnemyMemory,
            EnnemyPickableMemory ennemyPickableMemory)
            : base(
                  mover,
                  handController,
                  ennemyEnnemyMemory,
                  ennemyPickableMemory)
        {
        }

        override public void Act()
        {
            mover.Rotate(Mover.Clockwise);

            handController.Use();
        }

        protected override void Think()
        {
            
        }
    }
}