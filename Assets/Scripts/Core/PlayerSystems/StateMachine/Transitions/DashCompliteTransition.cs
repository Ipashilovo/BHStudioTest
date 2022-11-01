using Core.PlayerSystems.StateMachine.States;

namespace Core.PlayerSystems.StateMachine.Transitions
{
    public class DashCompliteTransition : AbstractTransition<DashState, IState>
    {
        public DashCompliteTransition(DashState previousState, IState nextState) : base(previousState, nextState)
        {
        }

        public override void Enable()
        {
            _previousState.Complited += ChangeState;
        }

        public override void Disable()
        {
            _previousState.Complited -= ChangeState;
        }
    }
}