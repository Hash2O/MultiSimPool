using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Unity.VisualScripting;

public class WhiteBall : NetworkBehaviour
{
    [SerializeField] 
    private float _angularDrag = 0.6f;

    [SerializeField] 
    private float _drag = 0.6f;
    
    private Rigidbody _rigidbody;

    private GameObject _playingCam;

    private AudioSource _audioSource;

    [SerializeField]
    private List<AudioClip> _audioClips;


    private void Awake()
    {
        Debug.Log("white ball awake");
        _rigidbody = GetComponent<Rigidbody>();

        _playingCam = GameObject.Find("PoolCam");
        Debug.Log("Cam : " + _playingCam);

        _audioSource = GetComponent<AudioSource>();

    }
    
    private IEnumerator Start()
    {
        while (PlayerManager.Instance == null)
        {
            yield return null;
        }
        PlayerManager.Instance.OnPlayerTurnChanged.AddListener(OnPlayerTurnChanged);
    }

    private void Update()
    {
        //if ball goes off the table
        if(gameObject.transform.position.y < - 0.75f)
        {
            //Respawn at new coordinates
            transform.position = new Vector3(0, 0.1f, 0);

            //Reduction of velocity to freeze the ball
            _rigidbody.velocity = Vector3.zero;

            //UX : sound to warn the player
            _audioSource.PlayOneShot(_audioClips[1]);
        }

    }

    private void OnPlayerTurnChanged(Player newPlayer)
    {
        
    }

    public override void Spawned()
    {
        Debug.Log("white ball spawned");
       
        //Camera.main.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource()
           // { sourceTransform = transform, weight = 1f });
        
        PlayerManager.Instance.Rpc_LoadDone();

        //Jingle (UX)
        _audioSource.PlayOneShot(_audioClips[1]);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_PushBall(Vector3 direction)
    {
        //GetComponent<Rigidbody>().AddForce(direction);
        
        StartCoroutine(CheckStopMoving());
    }

    public void OnMouseUpAsButton()
    {
        if (!PlayerManager.Instance.IsActivePlayerLocalPlayer())
            return;
        
        //Vector3 direction = Random.onUnitSphere;
        Vector3 direction = _playingCam.transform.forward;
        direction.y = 0;
        direction.Normalize();
        
        Rpc_PushBall(direction * 2);

        //UX : Impact noise
        _audioSource.PlayOneShot(_audioClips[0]);
    }

    private IEnumerator CheckStopMoving()
    {
        while (!_rigidbody.IsSleeping())
        {
            yield return null;
        }

        PlayerManager.Instance.EndPlayerTurn();
    }


}
