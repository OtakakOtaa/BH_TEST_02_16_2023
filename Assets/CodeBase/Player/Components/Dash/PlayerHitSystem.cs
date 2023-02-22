using System;
using System.Collections;
using CodeBase.Client_Server;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Player.Components.Dash
{
    public class PlayerHitSystem
    {
        private readonly Settings _settings;
        private readonly ICoroutineHolder _coroutineHolder;
        private readonly SkinnedMeshRenderer _meshRenderer;
        private readonly PlayerDash _playerDash;

        private Color _originColor;
        private bool _invulnerabilityFlag;

        public event Action<bool, Color> InvulnerabilityChanged;
        public event Action<int> HitCounterChanged;
        public event Action<Client> PlayerHited;
        
        public int HitCount { get; private set; }

        public PlayerHitSystem(ICoroutineHolder coroutineHolder, PlayerDash playerDash,
            SkinnedMeshRenderer renderer, Settings settings)
        {
            _settings = settings;
            _playerDash = playerDash;
            _coroutineHolder = coroutineHolder;
            _meshRenderer = renderer;
            _originColor = renderer.material.color;

            _playerDash.DashStarted += (isHit, hit) => { if (isHit) TryHit(hit); };
        }

        public void IncreaseHitCounter()
        {
            HitCount++;
            HitCounterChanged?.Invoke(HitCount);
        }

        public void ResetHitCounter()
        {
            HitCount = default;
            HitCounterChanged?.Invoke(HitCount);
        } 

        public void EnableInvulnerability()
            => _coroutineHolder.ExecuteCoroutine(InvulnerabilityDelayCoroutine);

        private IEnumerator InvulnerabilityDelayCoroutine()
        {
            ActiveInvulnerabilityColor();
            SetInvulnerabilityFlag(true);
            
            yield return new WaitForSeconds(_settings.InvulnerabilityTime);

            DisableInvulnerabilityColor();
            SetInvulnerabilityFlag(false);
        }

        private void SetInvulnerabilityFlag(bool value)
        {
            _invulnerabilityFlag = value;
            _playerDash.CanDash = !value;
            InvulnerabilityChanged?.Invoke(_invulnerabilityFlag, _meshRenderer.material.color);
        }

        private void TryHit(RaycastHit hitInfo)
        {
            bool isClient = hitInfo.transform.TryGetComponent<Client>(out var client);
            
            if (isClient && !client.IsInvulnerability && _playerDash.IsPlayerDashing) 
                PlayerHited?.Invoke(client);
        }

        private void ActiveInvulnerabilityColor()
        {
            _originColor = _meshRenderer.material.color;
            _meshRenderer.material.color = _settings.InvulnerabilityColor;
        }

        private void DisableInvulnerabilityColor()
        {
            _meshRenderer.material.color = _originColor;
        }
        
        [Serializable] public class Settings
        {
            [SerializeField, Range(2, 6)] private float _invulnerabilityTime = 3;
            [SerializeField] private Color _invulnerabilityColor = Color.red;

            public float InvulnerabilityTime => _invulnerabilityTime;
            public Color InvulnerabilityColor => _invulnerabilityColor;
        }
    }
}