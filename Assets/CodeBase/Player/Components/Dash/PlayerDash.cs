using System;
using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Player.Components.Dash
{
    public class PlayerDash
    {
        private readonly Settings _settings;
        private readonly ICoroutineHolder _coroutineHolder;
        private readonly PlayerInput _playerInput;
        private readonly Transform _player;

        public event Action<bool, RaycastHit> DashStarted;

        public bool CanDash { get; set; } = true;
        public bool IsPlayerDashing { get; private set; }

        public PlayerDash(Transform player, ICoroutineHolder coroutineHolder, PlayerInput playerInput,
            Settings settings)
        {
            _coroutineHolder = coroutineHolder;
            _playerInput = playerInput;
            _settings = settings;
            _player = player;

            _playerInput.LmbPressed += () =>
            {
                if (IsPlayerDashing is false && CanDash) coroutineHolder.ExecuteCoroutine(StartDashCoroutine);
            };
        }

        private IEnumerator StartDashCoroutine()
        {
            IsPlayerDashing = true;
            
            var position = _player.position;
            Vector3 startPosition = position;
            Vector3 endPosition = position + _player.forward * _settings.DashLength;

            var ray = new Ray(startPosition, endPosition - startPosition);
            bool isHit = Physics.Raycast(ray, out var hit, Vector3.Magnitude(endPosition - startPosition));

            DashStarted?.Invoke(isHit, hit);

            Vector3 stopPosition = Vector3.zero;
            if (isHit) stopPosition = hit.point;

            var duration = _settings.DashLength / _settings.Speed;
            const float  dopDistance = 0.4f;

            for (float i = 0; i < 1; i += Time.deltaTime / duration)
            {
                bool isCollision = isHit && Vector3.Magnitude(stopPosition - _player.position) <= dopDistance;
                if (isCollision) break;
                _player.position = Vector3.Lerp(startPosition, endPosition, i);
                yield return null;
            }

            IsPlayerDashing = false;
        }

        [Serializable]
        public class Settings
        {
            [SerializeField, Range(3, 6)] private float _dashLength;
            [SerializeField, Range(2, 6)] private float _speedCof;

            public float DashLength => _dashLength;
            public float Speed => _dashLength * _speedCof;
        }
    }
}