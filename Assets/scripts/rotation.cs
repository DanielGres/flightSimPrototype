using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    Vector3 pitchRot;
    Vector3 rollRot;
    Vector3 yawRot;
    public float pitchRotationSpeed;
    public float rollRotationSpeed;
    public float yawRotationSpeed;

    Rigidbody wing;

    public string key2;
    public string key3;
    public string key4;
    public string key5;
    public string key6;
    public string key7;

    void Start()
    {
        wing = GetComponent<Rigidbody>();
        pitchRot = new Vector3(pitchRotationSpeed, 0, 0);
        rollRot = new Vector3(0, 0, rollRotationSpeed);
        yawRot = new Vector3(0, rollRotationSpeed,0);
    }
    void Update()
    {
        float speed = GetComponent<Rigidbody>().velocity.magnitude;
        if (Input.GetKey(key2))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(pitchRot * Time.deltaTime * speed));
        }
        if (Input.GetKey(key3))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(-pitchRot * Time.deltaTime * speed));
        }
        if (Input.GetKey(key4))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(rollRot * Time.deltaTime * speed));
        }
        if (Input.GetKey(key5))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(-rollRot * Time.deltaTime * speed));
        }
        if (Input.GetKey(key6))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(yawRot * Time.deltaTime * speed));
        }
        if (Input.GetKey(key7))
        {
            wing.MoveRotation(wing.rotation * Quaternion.Euler(-yawRot * Time.deltaTime * speed));
        }
    }
}
