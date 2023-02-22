using CodeBase.Player.Components;
using CodeBase.Player.Components.Dash;
using CodeBase.Player.Components.View;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerContainer
    {
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        public NickNameView NickNameView;
        public PlayerMover PlayerMover;
        public ThirdPersonCameraBehavior ThirdPersonCameraBehavior;
        public PlayerInput PlayerInput;
        public PlayerAnimator PlayerAnimator;
        public PlayerHitSystem PlayerHitSystem;
        public PlayerDash PlayerDash;
    }
}