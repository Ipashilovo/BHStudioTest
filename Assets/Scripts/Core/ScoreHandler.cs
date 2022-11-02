using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Balance.Data;
using Core.PlayerSystems;
using Entities;
using Mirror;
using UnityEngine;

namespace Core
{
    public class ScoreHandler : NetworkBehaviour
    {
        private readonly SyncDictionary<string, int> _syncScores = new SyncDictionary<string, int>();
        private Dictionary<string, int> _score = new Dictionary<string, int>();
        [SerializeField] private EndGameConfig _endGameConfig;
        private int _id;
        [SyncVar(hook = nameof(SyncEndState))] private bool _SyncEndState;
        [SyncVar(hook = nameof(SyncWinner))] private string _SyncWinner;
        private string _winnerId;
        private IPlayerHandler _playerHandler;

        public bool IsEnded { get; private set; }
        public event Action<string> FindedWinner;
        public event Action<string> RemovedClient;
        public event Action<string, int> ScoreChanged;

        public override void OnStartClient()
        {
            _syncScores.Callback += OnSyncScoreChanged;
            foreach (var score in _syncScores)
            {
                OnSyncScoreChanged(SyncDictionary<string, int>.Operation.OP_ADD, score.Key, score.Value);
            }
        }
        
        public void Init(IPlayerHandler playerHandler)
        {
            _playerHandler = playerHandler;
            playerHandler.AddedNewPlayer += OnAddId;
            playerHandler.ScoreAdded += OnScoreAdd;
            playerHandler.RemovedPlayer += OnRemoveId;
        }
        
        public IReadOnlyDictionary<string, int> GetScore()
        {
            return _score;
        }


        private void OnRemoveId(UnitId obj)
        {
            if (_score.TryGetValue(obj.Value, out var value))
            {
                RemoveId(obj.Value);
            }
        }

        [Command(requiresAuthority = false)]
        private void RemoveId(string objValue) => RemoveToServer(objValue);

        [Server]
        private void RemoveToServer(string objValue) => _syncScores.Remove(objValue);

        private void OnScoreAdd(UnitId obj) => CommandAddScore(obj.Value);

        private void OnAddId(UnitId obj)
        {
            if (_score.TryGetValue(obj.Value, out var value))
            {
                return;
            }
            AddId(obj.Value);
        }

        private void OnDestroy()
        {
            if (_playerHandler != null)
            {
                _playerHandler.AddedNewPlayer -= OnAddId;
            }
        }

        private void OnSyncScoreChanged(SyncIDictionary<string, int>.Operation op, string key, int item)
        {
            switch (op)
            {
                case SyncIDictionary<string, int>.Operation.OP_ADD:
                    _score[key] = item;
                    ScoreChanged?.Invoke(key, item);
                    break;
                case SyncIDictionary<string, int>.Operation.OP_SET:
                    _score[key] = item;
                    ScoreChanged?.Invoke(key, item);
                    break;
                case SyncIDictionary<string, int>.Operation.OP_REMOVE:
                    _score.Remove(key);
                    RemovedClient?.Invoke(key);
                    break;
                case SyncIDictionary<string, int>.Operation.OP_CLEAR:
                    _score.Clear();
                    break;
            }
        }


        [Command(requiresAuthority = false)]
        private void AddId(string id) => AddIdToServer(id);

        [Server]
        private void AddIdToServer(string id) => _syncScores.Add(id, 0);

        [Command(requiresAuthority = false)]
        private void CommandAddScore(string unitId) => AddScore(unitId);

        [Server]
        private void AddScore(string id)
        {
            if (IsEnded)
            {
                return;
            }
            _syncScores[id] += 1;
            if (_syncScores[id] >= _endGameConfig.MaxScore)
            {
                _SyncEndState = true;
                SyncWinnerCommand(id);
                StartCoroutine(Restart());
            }
        }

        private IEnumerator Restart()
        {
            float time = 0;
            while (time <= _endGameConfig.DelayTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            NetworkClient.Send(new RespaunMassage());
            SyncWinnerCommand(null);
            SetEndState(false);
            DropScoreCommand();
        }

        [Command(requiresAuthority = false)]
        private void DropScoreCommand()
        {
            var keys = _syncScores.Keys.ToArray();
            foreach (var key in keys)
            {
                DropKey(key);
            }
        }

        [Server]
        private void DropKey(string key) => _syncScores[key] = 0;

        [Command(requiresAuthority = false)]
        private void SyncWinnerCommand(string id) => SyncWinner(id);

        [Server]
        private void SyncWinner(string id) => _SyncWinner = id;

        [Server]
        private void SyncEndState(bool state) => _SyncEndState = state;

        [Command(requiresAuthority = false)]
        private void SetEndState(bool ended) => SyncEndState(ended);

        private void SyncEndState(bool oldValue, bool newValue) => IsEnded = newValue;

        private void SyncWinner(string oldValue, string newValue)
        {
            _winnerId = newValue;
            if (String.IsNullOrEmpty(newValue) == false)
            {
                FindedWinner?.Invoke(_winnerId);
                return;
            }
        }
    }
}