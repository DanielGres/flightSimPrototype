using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiftGenerator : MonoBehaviour
{
    public GameObject fuselage;

    public float pitchControl = 0;
    public float pitchControlMax = 50;
    public float trimControl = 0;
    public float trimControlMax = 10;

    public string controlsup;
    public string controlsdown;

    public float liftMul = 10f;
    public float dragMul = 10f;
    public float gravity = 50;
    public float liftYaw = 5f;

    public AnimationCurve lift;
    float liftCoefficient;
    public AnimationCurve drag;
    float dragCoefficient;
    public AnimationCurve yawLift;
    float yawCoefficient;

    public float speed;
    Vector3 lastVelocity;
    
    public TextMeshProUGUI AOAMeter;
    public TextMeshProUGUI yawAngleMeter;
    public TextMeshProUGUI speedoMeter;
    public TextMeshProUGUI heightMeter;
    public TextMeshProUGUI ThrustMeter;

    public bool isFlap = true;
    public bool isYaw = false;
    public bool isVisualized = false;

    Rigidbody wing;
    thrustforce aircraft;
    Vector3 previous;
    public float LiftOffSet = 0;
    void Start()
    {
        aircraft = fuselage.GetComponent<thrustforce>();
        previous = transform.position;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(controlsup)) {
            if (Input.GetKey("t"))
            {
                trimControl += 0.3f;
            }
            else
            {
                pitchControl += 2.5f;

            }
        }
        if (Input.GetKey(controlsdown)) {
            if (Input.GetKey("t"))
            {
                trimControl -= 0.3f;
            }
            else
            {
                pitchControl -= 2.5f;

            }
        }
        Rotate(pitchControl, trimControl);

        var invRotation = Quaternion.Inverse(transform.rotation);
        Vector3 Velocity = (transform.position - previous) / Time.deltaTime;
        speed = Velocity.magnitude;
        previous = transform.position;
        Vector3 LocalVelocity = invRotation * Velocity;
        
        float height = transform.position.y;

        float angle = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z) * Mathf.Rad2Deg;
        float angleYaw = Mathf.Atan2(-LocalVelocity.x, LocalVelocity.z) * Mathf.Rad2Deg;

        pitchControl = Mathf.Clamp(pitchControl, -pitchControlMax / 2, pitchControlMax / 2);
        trimControl = Mathf.Clamp(trimControl, -trimControlMax / 2, trimControlMax / 2);
        pitchControl = pitchControl * 0.9f;
        liftCoefficient = lift.Evaluate(angle);
        dragCoefficient = drag.Evaluate(angle);

        yawCoefficient = yawLift.Evaluate(angleYaw);

        float liftForce = 0.5f * speed * speed * liftCoefficient * liftMul;
        float dragForce = 0.5f * speed * speed * dragCoefficient * dragMul;
        float yawForce = 0.5f * speed * speed * yawCoefficient * liftYaw;

        aircraft.forceWings(transform.up * liftForce * Time.deltaTime,transform.position + transform.forward * LiftOffSet);
        aircraft.forceWings(Vector3.down * gravity * Time.deltaTime, transform.position + transform.forward * LiftOffSet);
        aircraft.forceWings(transform.right * yawForce * Time.deltaTime, transform.position + transform.forward * LiftOffSet);
        aircraft.forceRelativeWings(LocalVelocity.normalized * -dragForce * Time.deltaTime + transform.forward * LiftOffSet);

        if (isVisualized)
        {
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, transform.up, Color.green, liftForce / 300);
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, -Velocity, Color.red, dragForce / 50);
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, Velocity, Color.blue, speed / 50);
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, transform.forward * speed, Color.magenta, speed / 400);
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, Vector3.down, Color.black, gravity / 300);
            DrawArrow.ForDebug(transform.position + transform.forward * LiftOffSet, transform.right, Color.cyan, yawForce / 200);

            AOAMeter.SetText("alpha: " + angle.ToString());
            yawAngleMeter.SetText("Yaw Angle: " + angleYaw.ToString());
            speedoMeter.SetText("velocity: " + speed.ToString());
            heightMeter.SetText("height: " + height.ToString());

            Color color;

            if (Mathf.Abs(angle) > 12)
            {
                color = Color.red;
            }
            else if (Mathf.Abs(angle) > 5)
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.green;
            }

            AOAMeter.color = color;
            yawAngleMeter.color = color;
            speedoMeter.color = color;
            heightMeter.color = color;
            
           
        }
        
    }

    public void Rotate(float pitchControl, float trimControl)
    {
        pitchControl = Mathf.Clamp(pitchControl, -pitchControlMax / 2, pitchControlMax / 2);
        trimControl = Mathf.Clamp(trimControl, -trimControlMax / 2, trimControlMax / 2);

        transform.localRotation = Quaternion.Euler(new Vector3(-pitchControl - trimControl, 0, 0));
        if (isYaw)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, -pitchControl - trimControl, 0));
        }
    }
    static void RotateAround(Transform transform, Vector3 pivotPoint, Vector3 axis, float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;
    }
}


