using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class thrustforce : MonoBehaviour
{
    Rigidbody propeler;

    public float headStart = 100f;
    public static float thrustForce = 1f;
    public float max_thrust_force = 150;
    public float thrustCoef = 1;

    public TextMeshProUGUI ThrustMeter;
    void Start()
    {
        propeler = GetComponent<Rigidbody>();
        propeler.AddForce(transform.forward * headStart * 100000 * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            thrustForce += 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {

            thrustForce -= 1f;
        }


        thrustForce = Mathf.Clamp(thrustForce, 0, max_thrust_force);
        propeler.AddForce(transform.forward * thrustForce * thrustCoef * Time.deltaTime);
        ThrustMeter.SetText("thrust: " + thrustForce.ToString());

        DrawArrow.ForDebug(transform.position, transform.forward, Color.magenta, thrustForce * thrustCoef / 50);
    }

    public void forceWings(Vector3 force,Vector3 position)
    {
        propeler.AddForceAtPosition(force, position);
    }
    public void forceRelativeWings(Vector3 force)
    {
        propeler.AddRelativeForce(force);
    }
}
