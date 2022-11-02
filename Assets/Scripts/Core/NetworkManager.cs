using System;
using System.Collections.Generic;
using Balance.Configs;
using Core.PlayerSystems;
using Entities;
using Mirror;
using UnityEngine;

namespace Core
{
    public class NetworkManager : Mirror.NetworkManager, IPlayerHandler
    {
        [SerializeField] private PlayerCongif _playerCongif;
        [SerializeField] private SpawnPositionFolder _positionFolder;
        public HashSet<UnitId> _unitIds = new HashSet<UnitId>();
        private List<PlayerModel> _playerModels;
        private ScoreHandler _scoreHandler;

        
        public event Action<UnitId> AddedNewPlayer;
        public event Action<UnitId> RemovedPlayer;
        public event Action<UnitId> ScoreAdded;

        public override void OnStartServer()
        {
            CreateScoreHandler();
            NetworkServer.RegisterHandler<PositionMassage>(OnCreateCharacter);
            NetworkServer.RegisterHandler<UnitIdMassage>(OnSetId);
            NetworkServer.RegisterHandler<ScoreAddMassage>(OnScoreAdd);
            NetworkServer.RegisterHandler<RespaunMassage>(Respaun);
            NetworkServer.RegisterHandler<RemovePlayerMassage>((args1, massage) => {RemovedPlayer?.Invoke(massage.UnitId);});
        }

        private void Respaun(NetworkConnectionToClient arg1, RespaunMassage arg2)
        {
            Debug.Log(_unitIds.Count);
            foreach (var unitId in _unitIds)
            {
                var position = _positionFolder.GetRandomPosition();
                NetworkServer.SendToAll(new RespaunPositionMassage()
                {
                    Position = position,
                    UnitId = unitId
                });
            }
        }

        private void OnScoreAdd(NetworkConnectionToClient arg1, ScoreAddMassage arg2)
        {
            ScoreAdded?.Invoke(arg2.Id);
        }

        private void OnSetId(NetworkConnectionToClient arg1, UnitIdMassage arg2)
        {
            _unitIds.Add(arg2.Id);
            AddedNewPlayer?.Invoke(arg2.Id);
        }

        private void CreateScoreHandler()
        {
            _scoreHandler = Instantiate(spawnPrefabs[0]).GetComponent<ScoreHandler>();
            _scoreHandler.Init(this);
            NetworkServer.Spawn(_scoreHandler.gameObject);
        }


        public override void OnClientConnect()
        {
            base.OnClientConnect();
            var positionMassage = new PositionMassage()
            {
                Position = _positionFolder.GetRandomPosition()
            };
            NetworkClient.Send(positionMassage);
        }

        private void OnCreateCharacter(NetworkConnectionToClient connection, PositionMassage massage)
        {
            var player =  Instantiate(playerPrefab, massage.Position,
                Quaternion.identity); 
            NetworkServer.AddPlayerForConnection(connection, player);
        }
    }

    public interface IPlayerHandler
    {
        public event Action<UnitId> AddedNewPlayer;
        public event Action<UnitId> ScoreAdded;
        public event Action<UnitId> RemovedPlayer;
    }
}