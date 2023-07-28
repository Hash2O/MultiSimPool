using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] 
    private GameObject _whiteBallPrefab;
    public static GameState State => Instance._gameState;
    private GameState _gameState;
    
    [Networked(OnChanged = nameof(ActivePlayerChanged))]
    public Player ActivePlayer { get; private set; }

    public UnityEvent<Player> OnPlayerTurnChanged = new();

    private List<Player> _players = new();

    private List<Player> _playerTurns = new();
    
    //singleton
    private static PlayerManager _instance;
    
    public static PlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public override void Spawned()
    {
        Debug.Log("PlayerManager spawned");
        _gameState = GetComponent<GameState>();

        //singleton
        if (_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public bool All(System.Predicate<Player> match)
    {
        return _players.Count(p => !match.Invoke(p)) == 0;
    }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void Rpc_LoadDone(RpcInfo info = default)
    {
        Player player = _players.First( p => p.NetworkPlayerRef == info.Source);
        player.IsLoaded = true;
        Debug.Log($"Player {player.PlayerName} has loaded.");
    }

    public void RegisterPlayer(Player player)
    {
        _players.Add(player);
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }

    public bool IsActivePlayerLocalPlayer()
    {
        if (ActivePlayer == null)
            return false;

        if (ActivePlayer.IsLocalPlayer())
            return true;

        return false;
    }

    public void StartGame()
    {
        foreach (var player in _players)
        {
            _playerTurns.Add(player);
        }
        //shuffle to have a random player start the game
        _playerTurns.Shuffle();

        NextTurn();
    }

    public void EndPlayerTurn(bool fault = false)
    {
        //add player again at the end of the list
        _playerTurns.Add(ActivePlayer);
        
        //and remove actual turn
        _playerTurns.RemoveAt(0);

        if (fault)
        {
            //add a second turn for next player
            _playerTurns.Insert(0, _playerTurns.First());
        }

        NextTurn();
    }
    
    private void NextTurn()
    {
        ActivePlayer = _playerTurns.FirstOrDefault();
    }

    public void SpawnWhiteBall()
    {
        Runner.Spawn(_whiteBallPrefab);
    }

    //network changed callbacks
    static void ActivePlayerChanged(Changed<PlayerManager> p)
    {
        p.Behaviour.OnPlayerTurnChanged?.Invoke(p.Behaviour.ActivePlayer);
    }
}
