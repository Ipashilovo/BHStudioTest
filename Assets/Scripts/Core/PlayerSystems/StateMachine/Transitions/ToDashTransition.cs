using UnityEngine;

namespace Core.PlayerSystems.StateMachine.Transitions
{
    public class ToDashTransition : AbstractTransition<IState, IState>
    {
        public ToDashTransition(IState previousState, IState nextState) : base(previousState, nextState)
        {
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeState();
            }
        }

        public override void Enable()
        {
            
        }

        public override void Disable()
        {
        }
    }
}