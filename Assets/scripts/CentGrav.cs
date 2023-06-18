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
        aircraft.forceWings(Vector3.down * 10 * gravity * Time.deltaTime, transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
