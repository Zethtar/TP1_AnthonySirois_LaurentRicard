
using Playmode.Weapon;

namespace Playmode.Ennemy.Strategies
{
    public enum EnnemyState
    {
        Idle,
        Attacking,
        Roaming
    }

    public enum EnnemyStrategy
    {
        Normal,
        Careful,
        Cowboy,
        Camper
    }

    public interface IEnnemyStrategy
    {
        void Act();
        void SetState(EnnemyState state);
        void SetEnnemyTarget(EnnemyController ennemyTarget);
        void SetWeaponTarget(WeaponController weaponTarget);
    }

  
}