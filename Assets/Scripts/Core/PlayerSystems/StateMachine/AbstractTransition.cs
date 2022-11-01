namespace Core.PlayerSystems.StateMachine
{
    public abstract class AbstractTransition<TPreviousState, TNextState> : ITransition where TNextState : IState where TPreviousState : IState
    {
        protected readonly TPreviousState _previousState;
        protected readonly TNextState _nextState;

        public AbstractTransition(TPreviousState previousState, TNextState nextState)
        {
            _previousState = previousState;
            _nextState = nextState;
        }

        public abstract void Enable();

        public abstract void Disable();
        public virtual void Update(){}

        protected void ChangeState()
        {
            _previousState.Exit();
            _nextState.Enter();
        }
    }
}