
using Playmode.Weapon;

namespace Playmode.Ennemy.Strategies
{
    public enum EnnemyState
    {
        Idle,
        Attacking,
        Camping,
        Roaming,
        MedkitGathering,
        MedkitSearching,
        WeaponSearching
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
    }

  
}