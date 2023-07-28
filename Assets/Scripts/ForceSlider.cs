using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSlider : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public Slider forceSlider;
    public float maxForce = 10f;
    public float maxClickDuration = 1f;

    private bool isClicking = false;
    private float clickStartTime;
    private float currentForce;

    public Camera cam;

    private void Start()
    {
        //Initialisation des variables
        cam = GameObject.Find("PoolCam").GetComponent<Camera>();
        forceSlider = GameObject.Find("Force Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast pour s'assurer que le GO balle est bien ciblé par le curseur 
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Début du clic sur la balle
                isClicking = true;
                clickStartTime = Time.time;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isClicking)
            {
                // Fin du clic sur la balle, appliquer la force sur le Rigidbody
                float clickDuration = Time.time - clickStartTime;
                currentForce = Mathf.Lerp(0f, maxForce, clickDuration / maxClickDuration);
                targetRigidbody.AddForce(cam.transform.forward * currentForce, ForceMode.Impulse);

                // Réinitialiser le slider et l'état de clic
                forceSlider.value = 0f;
                isClicking = false;
            }
        }

        if (isClicking)
        {
            // Mettre à jour le slider en fonction de la durée du clic
            float clickDuration = Time.time - clickStartTime;
            currentForce = Mathf.Lerp(0f, maxForce, clickDuration / maxClickDuration);
            forceSlider.value = clickDuration / maxClickDuration;
        }
    }
}
