using CodeBase.Client_Server;
using CodeBase.Infrastructure;
using CodeBase.Player.Components;
using CodeBase.Player.Components.Dash;
using CodeBase.Player.Components.View;
using UnityEngine;

namespace CodeBase.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Client))]
    [RequireComponent(typeof(Animator))]
    public class PlayerInstaller : MonoBehaviour, ICoroutineHolder
    {
        [SerializeField] private PlayerConfig _playerConfig;

        [SerializeField] private NickNameView _nickNameView;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;

        private Client _client;

        public PlayerCycle PlayerCycle { get; private set; }

        private void Start() => Install();

        private void Update()
        {
            if (_client.ClientHasAuthority is false || _client.IsClientShotDown) return;
            PlayerCycle?.UpdateState();
        }

        private void LateUpdate()
        {
            if (_client.ClientHasAuthority is false || _client.IsClientShotDown) return;
            PlayerCycle?.UpdateLastState();
        }

        private void Install()
        {
            _client = GetComponent<Client>();

            PlayerContainer container;

            PlayerInput playerInput = new();
            
            PlayerMover playerMover =
                new(GetComponent<CharacterController>(), playerInput, _playerConfig.MoveSettings);
            
            PlayerDash playerDash =
                new(player: transform, coroutineHolder: this, playerInput, _playerConfig.DashSettings);
            
            PlayerHitSystem playerHitSystem =
                new(coroutineHolder: this, playerDash, _meshRenderer, _playerConfig.HitSystemSettings);
            
            PlayerAnimator playerAnimator =
                new(GetComponent<Animator>(), playerInput, player: transform, playerDash);
            
            ThirdPersonCameraBehavior thirdPersonCameraBehavior =
                new(person: transform, Camera.main, playerInput, _playerConfig.CameraSettings);

            _nickNameView.Init();
            _nickNameView.SetName(_client.NickName);

            container = new PlayerContainer(){
                SkinnedMeshRenderer = _meshRenderer,
                NickNameView = _nickNameView,
                PlayerMover = playerMover,
                ThirdPersonCameraBehavior = thirdPersonCameraBehavior,
                PlayerInput = playerInput,
                PlayerAnimator = playerAnimator,
                PlayerHitSystem = playerHitSystem,
                PlayerDash = playerDash
            };
            
            PlayerCycle = new PlayerCycle(container);
            
            BindModelRequest(container);
            _client.SetPlayerEventsBoard(PlayerCycle);
            _client.BindSyncUpdate(container);
        }

        private void BindModelRequest(PlayerContainer playerContainer)
        {
            playerContainer.PlayerHitSystem.PlayerHited += target => 
                _client.SendRequestForHitProcessing(_client, target);
        }
        
        public Coroutine ExecuteCoroutine(ICoroutineHolder.CoroutineMethod coroutineMethod)
            => StartCoroutine(coroutineMethod.Invoke());

        public void ShutDownCoroutine(Coroutine coroutine)
            => StopCoroutine(coroutine);
    }
}