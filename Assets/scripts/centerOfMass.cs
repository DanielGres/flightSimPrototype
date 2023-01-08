using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerOfMass : MonoBehaviour
{
    public GameObject[] assembly;
    Vector3 CoM = Vector3.zero;
    void Start()
    {
        
        float c = 0f;

        foreach (GameObject part in assembly)
        {
            CoM += part.GetComponent<Rigidbody>().worldCenterOfMass * part.GetComponent<Rigidbody>().mass;
            c += part.GetComponent<Rigidbody>().mass;
        }

        CoM /= c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CoM, 0.1f);
    }
}
