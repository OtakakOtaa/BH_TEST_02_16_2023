using System;
using System.Collections;
using CodeBase.GamePlayView;
using CodeBase.Player;
using CodeBase.SpawnPoints;
using Mirror;
using UnityEngine;

namespace CodeBase.Client_Server
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    public class Client : NetworkBehaviour
    {
        private const string ClientNameID = "ClientNameID";

        private PlayersScoreView _playerScoreView;
        private IPlayerEvents _playerEvents;
        
        [SyncVar] private int _hitCount;
        [SyncVar] private bool _isInvulnerability;
        [SyncVar] private string _nickName;
        [SyncVar] private bool _clientShotDown;

        private Coroutine _restartGameCoroutine;
        
        public int HitCount => _hitCount;
        public bool IsInvulnerability => _isInvulnerability;
        public bool ClientHasAuthority => isOwned;
        public bool IsClientShotDown => _clientShotDown;
        public string NickName => _nickName;

        public override void OnStartClient()
        {
            LoadNickName();
            SetTransform();
            InstantiateView();
        }

        public void Update()
        {
            if (_clientShotDown && _restartGameCoroutine is null)
                StartCoroutine(RestartGameDelay());
        }

        public void SetPlayerEventsBoard(IPlayerEvents playerEvents) 
            => _playerEvents = playerEvents;
        
        public void BindSyncUpdate(PlayerContainer container)
        {
            container.PlayerHitSystem.InvulnerabilityChanged += (flag, color) =>
            {
                _isInvulnerability = flag;
            };
            container.PlayerHitSystem.HitCounterChanged += counter => _hitCount = counter;
        }
        
        [Command] public void SendRequestForHitProcessing(Client hitter, Client target)
        {
            const int winnerScore = 3;
            var hitterIsWin = hitter._hitCount + 1 == winnerScore;
            if(hitterIsWin) 
                StartWinGameProcess(hitter);
            else
            {
                hitter._playerEvents.GivePoint();
                TakeDamageOnClient(target);
            }
        }

        [ClientRpc] private void TakeDamageOnClient(Client target)
        {
            _playerScoreView.UpdatePlayerScoreView();
            target._playerEvents.TakeDamage();
        }

        [ClientRpc] private void StartWinGameProcess(Client winner)
        {
            _playerScoreView.ShowWinPanel(winner.NickName);
            _hitCount = 0;
            _playerEvents.Restart();
            _playerScoreView.DiscardPlayersScore();
            _clientShotDown = true;
        }
        
        private IEnumerator RestartGameDelay()
        {
            const int restartDelay = 5;
            _clientShotDown = true;
            yield return new WaitForSeconds(restartDelay);
            
            _clientShotDown = false;
            _restartGameCoroutine = null;
            transform.position = FindObjectOfType<SpawnPointsProvider>().RandomizeSpawnPoints()[0];
            _playerScoreView.DiscardWinPanel();
        }

        private void SetTransform()
        {
            var syncTransform = GetComponent<NetworkTransform>();
            syncTransform.syncDirection = SyncDirection.ClientToServer;
            syncTransform.syncMode = SyncMode.Owner;
        }
        private void LoadNickName()
            => _nickName = PlayerPrefs.GetString(ClientNameID);

        private void InstantiateView()
        {
            _playerScoreView = FindObjectOfType<PlayersScoreView>();
            _playerScoreView.DetectAllPlayer();
            _playerScoreView.UpdatePlayerScoreView();
        }
    }
}