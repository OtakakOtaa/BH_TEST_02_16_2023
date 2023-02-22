using System;
using UnityEngine;

namespace CodeBase.Player.Components
{
    public class PlayerMover
    {
        private readonly Settings _settings;

        private readonly PlayerInput _playerInput;
        private readonly CharacterController _target;

        public PlayerMover(CharacterController player, PlayerInput playerInput, Settings settings)
        {
            _target = player;
            _playerInput = playerInput;
            _settings = settings;
        }

        public void UpdateState()
            => TryMove();

        private void TryMove()
        {
            if (_playerInput.IsMoving is false) return;

            Vector2 inputDirection = _playerInput.DirectionMovement;
            var transform = _target.transform;
            Vector3 forward = transform.forward * inputDirection.y;
            Vector3 side = transform.right * inputDirection.x;
            Vector3 directionMove = (forward + side).normalized * MoveSpeedDelta;

            _target.Move(directionMove);
        }

        private float MoveSpeedDelta
            => _settings.MoveSpeed * Time.deltaTime;

        [Serializable] public class Settings
        {
            [SerializeField, Range(1, 3)] private float _moveSpeed;
            public float MoveSpeed => _moveSpeed;
        }
    }
}