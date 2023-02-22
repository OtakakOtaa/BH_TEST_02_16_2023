using System.Linq;
using CodeBase.ConnectionToServer;
using CodeBase.SpawnPoints;
using Mirror;
using UnityEngine;

namespace CodeBase.Client_Server
{
    [RequireComponent(typeof(TelepathyTransport))]
    public class ServerCycle : NetworkManager
    {
        [SerializeField] private ConfirmWindow _confirmWindow;
        private const string ClientNameID = "ClientNameID";
        private bool _isServerInit;

        public override void Start()
        {
            transport = GetComponent<TelepathyTransport>();
            _confirmWindow.StartConnectionToServer += 
                (nickname, address) => PrepareToStart(isHostRole: false, nickname, address);
            _confirmWindow.StartServer += 
                (nickname, address) => PrepareToStart(isHostRole: true, nickname, address);
        }
        
        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            if(_isServerInit is false) InitServer();
        }
        
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject client = Instantiate(playerPrefab);
            client.transform.position = GetStartPosition().position;
            NetworkServer.AddPlayerForConnection(conn, client);
        }

        private void PrepareToStart(bool isHostRole, string nickname, string address)
        {
            PlayerPrefs.SetString(ClientNameID, nickname);

            if (isHostRole)
            {
                string host = System.Net.Dns.GetHostName();
                networkAddress = System.Net.Dns.GetHostByName(host).AddressList[0].ToString();
                StartHost();
                return;
            }
            networkAddress = address;
            StartClient();
        }
        
        [Server] private void InitServer()
        {
            var spawnPointsProvider = FindObjectOfType<SpawnPointsProvider>();
            
            startPositions = spawnPointsProvider.RandomizeSpawnPoints()
                .Select(position => new GameObject($"SpawnPoint {position}")
                    { transform = { position = position, parent = spawnPointsProvider.transform } }.transform
                ).ToList();
            playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
            _isServerInit = true;
        }
    }
}