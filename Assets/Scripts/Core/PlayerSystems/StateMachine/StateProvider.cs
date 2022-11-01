using System;

namespace Core.PlayerSystems.StateMachine
{
    public class StateProvider : IUpdatable, IDisposable
    {
        private IState _state;

        public void SetState(IState state)
        {
            _state = state;
        }
        
        
        public void Update()
        {
            _state?.Update();
        }

        public void Dispose()
        {
            _state?.Dispose();
        }
    }
}