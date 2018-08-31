namespace Playmode.Enemy.Strategies
{
    public enum EnnemyState
    {
        Idle,
        Attacking,
        Camping,
        Roaming,
        MedkitGathering,
        MedkitSearching,
        WeaponGathering
    }

    public enum EnemyStrategy
    {
        Normal,
        Careful,
        Cowboy,
        Camper
    }

    public interface IEnnemyStrategy
    {
        void Act();
    }
}