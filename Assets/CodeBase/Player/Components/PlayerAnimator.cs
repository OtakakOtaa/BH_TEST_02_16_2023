using CodeBase.Player.Components.Dash;
using UnityEngine;

namespace CodeBase.Player.Components
{
    public class PlayerAnimator
    {
        private readonly Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsDashed = Animator.StringToHash("IsDashed");
        private const int ForwardId = 1;
        private const int BackwardId = -1;
        private const int IdleId = 0;

        private readonly PlayerInput _playerInput;
        private readonly Transform _player;
        private readonly PlayerDash _playerDash;

        private Vector3 _lastPosition;
        
        public PlayerAnimator(Animator animator, PlayerInput playerInput, Transform player, PlayerDash playerDash)
        {
            _animator = animator;
            _playerInput = playerInput;
            _player = player;
            _playerDash = playerDash;
            _lastPosition = player.position;
        }

        public void ObserveMovement()
        {
            if (IsMoved)
                _animator.SetFloat(Speed, IsForward ? ForwardId : BackwardId);
            else
                _animator.SetFloat(Speed, IdleId);

            _animator.SetBool(IsDashed, _playerDash.IsPlayerDashing);
            
            _lastPosition = _player.position;
        }

        private bool IsMoved => _lastPosition != _player.position;
        private bool IsForward => _playerInput.DirectionMovement.y > 0;
    }
}