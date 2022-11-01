using System;
using System.Collections.Generic;

namespace Core.PlayerSystems.StateMachine
{
    public interface IState : IDisposable, IUpdatable
    {
        public void Enter();
        public void Exit();
    }

    public abstract class AbstractState : IState
    {
        private readonly StateProvider _stateProvider;
        private IEnumerable<ITransition> _transitions;

        public AbstractState(StateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }
        
        public void SetTransitions(IEnumerable<ITransition> transition)
        {
            _transitions = transition;
        }

        public virtual void Update()
        {
            if (_transitions != null)
            {
                foreach (var transition in _transitions)
                {
                    transition.Update();
                }
            }
        }

        public virtual void Dispose()
        {
            if (_transitions != null)
            {
                foreach (var transition in _transitions)
                {
                    transition.Disable();
                }
            }
        }

        public virtual void Enter()
        {
            _stateProvider.SetState(this);
            if (_transitions != null)
            {
                foreach (var transition in _transitions)
                {
                    transition.Enable();
                }
            }
        }

        public virtual void Exit()
        {
            if (_transitions != null)
            {
                foreach (var transition in _transitions)
                {
                    transition.Disable();
                }
            }
            Dispose();
        }
    }
}