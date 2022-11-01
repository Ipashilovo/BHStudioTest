using System.Collections.Generic;
using Balance.Configs;
using Core.PlayerSystems.Movement;
using Core.PlayerSystems.StateMachine;
using Core.PlayerSystems.StateMachine.States;
using Core.PlayerSystems.StateMachine.Transitions;
using UnityEngine;

namespace Core.PlayerSystems
{
    [CreateAssetMenu(fileName = "PlayerFactory", menuName = "ScriptableObject/PlayerFactory", order = 0)]
    public class PlayerFactory : ScriptableObject
    {
        [SerializeField] private PlayerCongif _playerCongif;
        public void Init(PlayerModel playerModel)
        {
            CameraMovement cameraMovement = new CameraMovement(playerModel.Camera, _playerCongif.GetCameraData());
            StateProvider stateProvider = new StateProvider();
            CreateStates(stateProvider, playerModel);
            playerModel.Init(new List<IUpdatable>()
            {
                cameraMovement,
                stateProvider
            });
        }

        private void CreateStates(StateProvider stateProvider, PlayerModel playerModel)
        {
            var playerMovementData = _playerCongif.GetMovementData();
            MoveState moveState = new MoveState(playerModel, playerMovementData, stateProvider);
            DashState dashState = new DashState(playerModel, playerMovementData, stateProvider);
            ToDashTransition toDashTransition = new ToDashTransition(moveState, dashState);
            DashCompliteTransition dashCompliteTransition = new DashCompliteTransition(dashState, moveState);
            moveState.SetTransitions(new []{toDashTransition});
            dashState.SetTransitions(new []{dashCompliteTransition});
            moveState.Enter();
        }
    }
}