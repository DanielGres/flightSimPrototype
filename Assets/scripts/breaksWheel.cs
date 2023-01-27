using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breaksWheel : MonoBehaviour
{
    public WheelCollider myWheel;
    public float breakForce;
    void Start()
    {
        myWheel = transform.GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        myWheel.brakeTorque = 0;
        if (Input.GetKey("b"))
        {
            myWheel.brakeTorque = breakForce;
        }


    }
}
