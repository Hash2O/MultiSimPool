using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


//Axe de reflexion : faire de la canne de billard un enfant de la camera de jeu (eviter les redondances)

public class BillardCueManager : MonoBehaviour
{
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
        transform.LookAt(_target.transform.position);

        if(Input.GetMouseButtonDown(0))
        {
            
        }
    }

    IEnumerator FindingTarget()
    {
        yield return new WaitForSeconds(3f);
        _target = GameObject.FindWithTag("Player");
        Debug.Log("White Ball acquired...");
    }

    IEnumerator StrikeTheBall()
    {
        transform.position = new Vector3(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(-1, 0, 0);
    }
}
