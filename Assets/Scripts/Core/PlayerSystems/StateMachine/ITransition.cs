namespace Core.PlayerSystems.StateMachine
{
    public interface ITransition : IUpdatable
    {
        public void Enable();
        public void Disable();
    }
}