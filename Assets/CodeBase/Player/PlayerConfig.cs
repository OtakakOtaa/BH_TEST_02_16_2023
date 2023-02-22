using CodeBase.Player.Components;
using CodeBase.Player.Components.Dash;
using UnityEngine;

namespace CodeBase.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Player Config", order = 1)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private PlayerMover.Settings _moveSettings;
        [SerializeField] private ThirdPersonCameraBehavior.Settings _cameraSettings;
        [SerializeField] private PlayerHitSystem.Settings _hitSystemSettings;
        [SerializeField] private PlayerDash.Settings _dashSettings;
        
        public ThirdPersonCameraBehavior.Settings CameraSettings => _cameraSettings;
        public PlayerMover.Settings MoveSettings => _moveSettings;
        public PlayerHitSystem.Settings HitSystemSettings => _hitSystemSettings;
        public PlayerDash.Settings DashSettings => _dashSettings;

    }
}