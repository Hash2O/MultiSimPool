using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickToShootTheBall : MonoBehaviour
{
    public Rigidbody _targetRigidbody;
    public Slider forceSlider;
    public float maxForce = 10f;
    public float maxClickDuration = 1f;

    private bool isClicking = false;
    private float clickStartTime;
    private float currentForce;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        if(_targetRigidbody == null)
        {
            _targetRigidbody = GetComponent<Rigidbody>();
        }
        
        StartCoroutine("FindingTargetRigidbody");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Début du clic de souris
            isClicking = true;
            clickStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isClicking)
            {
                // Fin du clic de souris, appliquer la force sur le Rigidbody
                float clickDuration = Time.time - clickStartTime;
                currentForce = Mathf.Lerp(0f, maxForce, clickDuration / maxClickDuration);
                _targetRigidbody.AddForce(cam.transform.forward * currentForce, ForceMode.Impulse);

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

    IEnumerator FindingTargetRigidbody()
    {
        yield return new WaitForSeconds(3f);
        _targetRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        Debug.Log("White Ball acquired...");
    }
}
