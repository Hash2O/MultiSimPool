using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Session : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private GameObject _playerManagerPrefab;
    [SerializeField]
    private GameObject _playerPrefab;
    
    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();
    
    private NetworkRunner _runner;

    public UnityEvent onSessionJoined;
    public UnityEvent onSessionLeft;

    public UnityEvent onReadyToStart;
    public bool IsRunning => _runner && _runner.IsRunning;
    
    public NetworkRunner Runner => _runner;
    void Awake()
    {
        _runner = GetComponent<NetworkRunner>();
        
        //persistent
        DontDestroyOnLoad(gameObject);
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId} joined.");
        if (runner.IsServer)
        {
            if(runner.LocalPlayer == player)
            {
                runner.Spawn(_playerManagerPrefab);
            }
            
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, inputAuthority: player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedPlayers.Add(player, networkPlayerObject);

            networkPlayerObject.GetComponent<Player>().NetworkPlayerRef = player;

            if (_spawnedPlayers.Count == 2)
            {
               PlayerManager.State.Server_SetState(GameState.EGameState.Loading);
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId} left.");
        
        if (runner.IsServer)
        {
            if (_spawnedPlayers.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedPlayers.Remove(player);
            }
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.IsServer && runner.SimulationUnityScene.name == "Game")
        {
            PlayerManager.Instance.SpawnWhiteBall();
        }
    }

    public async Task StartSession()
    {
        var result = await _runner.StartGame(new StartGameArgs() 
        {
            GameMode = GameMode.AutoHostOrClient,
            PlayerCount = 2,

        });

        if (result.Ok) 
        {
            // all good
            Debug.Log("room joined.");
            onSessionJoined?.Invoke();
 
        }
        else 
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            onSessionLeft?.Invoke();

        }
    }

    public async Task LeaveSession()
    {
        await _runner.Shutdown();
    }

}
