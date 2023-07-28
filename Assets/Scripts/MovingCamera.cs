using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private float _movingSpeed = 10.0f; //S�rialiser pour acc�lerer/d�ccel�rer les mouvements de la cam�ra
    private float _followDistance = 1.5f;   //S�rialiser pour acc�lerer/d�ccel�rer les mouvements de la cam�ra
    private float _speed = 2.5f;    //S�rialiser pour permettre au player de choisir la distance entre la camera et la balle
    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("PoolTableErgasia3 Variant");
        StartCoroutine("FindingTarget");
    }

    // Update is called once per frame
    void Update()
    {
        //Cam tourn�e vers la balle
        transform.LookAt(_target.transform.position);

        //Manoeuvrer la cam�ra
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.up * Time.deltaTime * _movingSpeed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * _movingSpeed * horizontalInput);

    }

    private void LateUpdate()
    {
        //Si la balle s'�loigne trop, la cam�ra se rapproche
        if (Vector3.Distance(transform.position, _target.transform.position) > _followDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }
    }

    IEnumerator FindingTarget()
    {
        yield return new WaitForSeconds(3f);
        _target = GameObject.FindWithTag("Player");
        Debug.Log("White Ball acquired...");
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }
}
