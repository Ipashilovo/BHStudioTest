using System;
using System.Collections;
using Balance.Data;
using Mirror;
using UnityEngine;

namespace Core.PlayerSystems
{
    public class PlayerInvulnerableHandler : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SyncInvulnerable))] private bool _SyncInvulnerable;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private  PlayerInvulnerableConfig _playerColorConfig;
        [SyncVar(hook = nameof(SyncColor))] private Color _SyncColor;
        private Coroutine _coroutime;
        private Material _material;
        private Color _baseColor;
        private bool _isInvulnerable;

        public bool IsInvulnerable => _isInvulnerable;

        private void Awake()
        {
            _material = _renderer.material;
            _baseColor = _material.color;
        }
        
        [Command(requiresAuthority = false)]
        public void Hit() => OnHit();

        [Server]
        public void OnHit() => SetHitState();

        [ClientRpc]
        private void SetHitState()
        {
            if (_coroutime != null)
            {
                StopCoroutine(_coroutime);
            }

            _coroutime = StartCoroutine(SetInvulnerableColor(_playerColorConfig.Time));
        }

        [Server]
        public void SyncInvulnerable(bool newValue) => _SyncInvulnerable = newValue;

        [Command]
        public void SetInvulnerable(bool newValue) => SyncInvulnerable(newValue);

        [Command]
        public void SetColor(Color color) => SyncColor(color);

        [Server]
        public void SyncColor(Color color) => _SyncColor = color;

        public void SyncColor(Color oldValue, Color newValue) => _material.color = newValue;
        private void SyncInvulnerable(bool oldValue, bool newValue) => _isInvulnerable = newValue;
        
        private IEnumerator SetInvulnerableColor(float time)
        {
            SetInvulnerable(true);
            SetColor(_playerColorConfig.Color);
            float currentTime = 0;
            while (time > currentTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            SetColor(_baseColor);
            SetInvulnerable(false);
            _coroutime = null;
        }
    }
}