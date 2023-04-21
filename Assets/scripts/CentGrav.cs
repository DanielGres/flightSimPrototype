using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentGrav : MonoBehaviour
{
    public GameObject fuselage;
    public float gravity = 50;
    thrustforce aircraft;

    void Start()
    {
        aircraft = fuselage.GetComponent<thrustforce>();
    }

    void Update()
    {
        aircraft.forceWings(Vector3.down * gravity * Time.deltaTime, transform.position);
    }
}
