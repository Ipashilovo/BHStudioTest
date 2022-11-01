using Balance.Data;
using UnityEngine;

namespace Core.PlayerSystems.StateMachine.States
{
    public class MoveState : AbstractState
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerMovementData _playerMovementData;

        public MoveState(PlayerModel playerModel, PlayerMovementData playerMovementData, StateProvider stateProvider) : base(stateProvider)
        {
            _playerModel = playerModel;
            _playerMovementData = playerMovementData;
        }

        public override void Update()
        {
            base.Update();

            var angleSpeed = Input.GetAxis("Mouse X") * Time.deltaTime * _playerMovementData.RotateSpeed;
            Transform transform = _playerModel.transform;
            transform.Rotate(0, angleSpeed, 0);
            Vector3 direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            direction *= Time.deltaTime * _playerMovementData.Speed;
            _playerModel.CharacterController.Move(direction);
        }
    }
}