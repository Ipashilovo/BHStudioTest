using Entities;

namespace Core.PlayerSystems
{
    public interface IUnit
    {
        public bool IsInvulnerable { get; }
        public UnitId Id { get; }
        public void Hit();
    }
}