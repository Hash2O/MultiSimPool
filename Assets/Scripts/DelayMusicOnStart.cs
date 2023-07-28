using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayMusicOnStart : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine("DelayMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(5f);
        _audioSource.Play();
    }
}
