using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;

    private bool isCamera1Active = true;

    private void Start()
    {
        //NB : on d�sactive une des deux cams au d�part pour permettre le switch par la suite
        camera1.gameObject.SetActive(isCamera1Active);
        camera2.gameObject.SetActive(!isCamera1Active);
    }

    public void SwitchCameras()
    {
        //true est remplac� par false, les deux cams changent d'�tat
        isCamera1Active = !isCamera1Active;
        camera1.gameObject.SetActive(isCamera1Active);
        camera2.gameObject.SetActive(!isCamera1Active);
    }

}
