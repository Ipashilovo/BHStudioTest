using System;
using System.Collections.Generic;
using Entities;
using Mirror;
using UnityEngine;

namespace Core.PlayerSystems
{
    public class PlayerModel : NetworkBehaviour, IUnit
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SpawnPositionFolder _spawnPositionFolder;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerFactory _factory;
        [SerializeField] private PlayerInvulnerableHandler _playerInvulnerableHandler;
        [SyncVar(hook = nameof(SyncId))] private string _SyncId;
        
        private List<IUpdatable> _updatables = new List<IUpdatable>();
        
        public UnitId Id { get; private set; }

        public bool IsInvulnerable => _playerInvulnerableHandler.IsInvulnerable;

        public Camera Camera => _camera;

        public PlayerInvulnerableHandler playerInvulnerableHandler => _playerInvulnerableHandler;

        public CharacterController CharacterController => _characterController;

        public event Action<PlayerModel> ScoreAdded;
        public event Action<IUnit> FindedTarget;

        private void Awake()
        {
            _factory.Init(this);
        }
        
        private void Start()
        {
            if (!isLocalPlayer)
            {
                _camera.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (hasAuthority == false)
            {
                return;
            }
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (isLocalPlayer)
            {
                SetId(new UnitId(netIdentity.netId.ToString()));
                NetworkClient.RegisterHandler<RespaunPositionMassage>(Respaun);
            }
        }

        private void Respaun(RespaunPositionMassage obj)
        {
            if (obj.UnitId == Id)
            {
                transform.position = obj.Position;
            }
        }


        public void Hit() => _playerInvulnerableHandler.Hit();
        public void Init(List<IUpdatable> updatables) => _updatables = updatables;

        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.TryGetComponent(out IUnit unit))
            {
                FindedTarget?.Invoke(unit);
            }
        }
        
        [Command]
        private void SetId(UnitId unitId)
        {
            SyncId(unitId.Value);
        }
        
        [Server]
        private void SyncId(string id)
        {
            _SyncId = id;
        }

        private void SyncId(string oldValue, string newValue)
        {
            Id = new UnitId(newValue);
            NetworkClient.Send(new UnitIdMassage()
            {
                Id = Id
            });
        }

        public void AddScore()
        {
            Debug.Log("AddScoreOnPlayer");
            NetworkClient.Send(new ScoreAddMassage()
            {
                Id = Id
            });
        }
    }
}