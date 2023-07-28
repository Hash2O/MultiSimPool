using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private Camera[] _children;
    // Start is called before the first frame update
    void Start()
    {
        _children = gameObject.GetComponentsInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleCameras()
    {
        foreach (var child in _children)
        {
            GameObject gm = child.gameObject;

            if (gm.activeSelf)
            {
                gm.SetActive(false);
            }
            else
            {
                gm.SetActive(true);
            }
        }

    }
}
