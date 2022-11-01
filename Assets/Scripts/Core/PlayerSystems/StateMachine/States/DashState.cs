using System;
using Balance.Data;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Core.PlayerSystems.StateMachine.States
{
    public class DashState : AbstractState
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerMovementData _playerMovementData;
        private float _distance;
        private Vector3 _previousPosition;

        public event Action Complited; 

        public DashState(PlayerModel playerModel, PlayerMovementData playerMovementData, StateProvider stateProvider) : base(stateProvider)
        {
            _playerModel = playerModel;
            _playerMovementData = playerMovementData;
        }

        public override void Enter()
        {
            base.Enter();
            _distance = 0;
            _previousPosition = _playerModel.transform.position;
            _playerModel.FindedTarget += OnHit;
        }

        public override void Update()
        {
            base.Update();
            _playerModel.CharacterController.Move(_playerModel.transform.forward *
                                                  (Time.deltaTime * _playerMovementData.DashSpeed));
            Vector3 currentPosition = _playerModel.transform.position;
            _distance += Vector3.Distance(currentPosition, _previousPosition);
            if (_distance >= _playerMovementData.DashDistance)
            {
                Complited?.Invoke();
            }
        }

        private void OnHit(IUnit obj)
        {
            if (obj.IsInvulnerable == false)
            {
                if (_playerModel.IsInvulnerable || String.IsNullOrEmpty(_playerModel.Id.Value))
                {
                    return;
                }
                obj.Hit();
                _playerModel.AddScore();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _playerModel.FindedTarget -= OnHit;
        }
    }
}