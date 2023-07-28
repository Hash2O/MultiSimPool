using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Networked]
    public PlayerRef NetworkPlayerRef { get; set; }

    [Networked]
    public bool IsLoaded { get; set; }

    public string PlayerName => $"Player {NetworkPlayerRef.PlayerId + 1} ";
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerManager.Instance.RegisterPlayer(this);
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.UnregisterPlayer(this);
    }

    public bool IsLocalPlayer()
    {
        return PlayerManager.Instance.Runner.LocalPlayer == NetworkPlayerRef;
    }
}
