namespace CodeBase.Player
{
    public class PlayerCycle : IPlayerEvents
    {
        private readonly PlayerContainer _playerContainer;
        
        public PlayerCycle(PlayerContainer playerContainer)
        {
            _playerContainer = playerContainer;
        }
        
        public void UpdateState()
        {
            _playerContainer.NickNameView.UpdateState();
            _playerContainer.PlayerInput.UpdateState();
            
            if (_playerContainer.PlayerDash.IsPlayerDashing) return;
            _playerContainer.PlayerMover.UpdateState();
            _playerContainer.ThirdPersonCameraBehavior.UpdateState();
        }

        public void UpdateLastState()
        {
            _playerContainer.ThirdPersonCameraBehavior.UpdateLastState();
            _playerContainer.PlayerAnimator.ObserveMovement();
        }

        public void GivePoint() 
            => _playerContainer.PlayerHitSystem.IncreaseHitCounter();

        public void TakeDamage()
            => _playerContainer.PlayerHitSystem.EnableInvulnerability();

        public void Restart()
        {
            _playerContainer.PlayerHitSystem.ResetHitCounter();
        }
    }
}