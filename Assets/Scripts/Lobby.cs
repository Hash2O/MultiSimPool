using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField]
    private GameObject _joinButton;
    
    [SerializeField]
    private TMP_Text _joinText;

    [SerializeField]
    private GameObject _sessionPrefab;

    private Session _currentSession;
    void Awake()
    {
        _joinText.text = "No game joined";
    }
    
    public async void OnCreateOrJoinGame()
    {
        if (_currentSession && _currentSession.IsRunning)
        {
            _joinButton.GetComponent<Button>().interactable = false;
            _joinText.text = "Leaving room...";
            await _currentSession.LeaveSession();
            _currentSession = null;
            _joinButton.GetComponent<Button>().interactable = true;
            _joinText.text = "No game joined";
            _joinButton.GetComponentInChildren<TMP_Text>().text = "Join Game";
        }
        else
        {
            _joinButton.GetComponent<Button>().interactable = false;
            _joinText.text = "Joining room...";

            _currentSession = Instantiate(_sessionPrefab).GetComponent<Session>();
            
            _currentSession.onSessionJoined.AddListener(OnSessionJoined);
            _currentSession.onSessionLeft.AddListener(OnSessionLeft);

            await _currentSession.StartSession();
        }
    }

    void OnSessionJoined()
    {
        _joinText.text = "Room joined, waiting for players...";
            
        _joinButton.GetComponent<Button>().interactable = true;
        _joinButton.GetComponentInChildren<TMP_Text>().text = "Leave Room";
    }

    void OnSessionLeft()
    {
        _joinButton.GetComponent<Button>().interactable = true;
        _joinText.text = "No game joined";
    }
}
