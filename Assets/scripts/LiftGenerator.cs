using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiftGenerator : MonoBehaviour
{
    public float pitchControl = 0;
    public float pitchControlMax = 50;

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

    Rigidbody wing;
    void Start()
    {
        wing = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey(controlsup)){ pitchControl += 1f; }
        if (Input.GetKey(controlsdown)){ pitchControl -= 1f; }
        pitchControl = Mathf.Clamp(pitchControl, -pitchControlMax / 2, pitchControlMax / 2);
        
        speed = wing.velocity.magnitude;

        var invRotation = Quaternion.Inverse(wing.rotation);
        Vector3 Velocity = wing.velocity;
        Vector3 LocalVelocity = invRotation * Velocity;

        float height = transform.position.y;

        float angle = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z) * Mathf.Rad2Deg;
        float angleYaw = Mathf.Atan2(-LocalVelocity.x, LocalVelocity.z) * Mathf.Rad2Deg;

        var acceleration = (Velocity - lastVelocity) / Time.deltaTime;
        Vector3 LocalGForce = invRotation * acceleration;
        lastVelocity = Velocity;

        pitchControl = pitchControl * 0.95f;


        liftCoefficient = lift.Evaluate(angle+pitchControl);
        dragCoefficient = drag.Evaluate(angle);
        yawCoefficient = yawLift.Evaluate(angleYaw);

        float liftForce = 0.5f * speed * speed * liftCoefficient * liftMul;
        float dragForce = 0.5f * speed * speed * dragCoefficient * dragMul;
        float yawForce = 0.5f * speed * speed * yawCoefficient * liftYaw;

        //Debug.Log(" liftCoefficient: " + liftCoefficient + " liftForce: " + liftForce + " dragCoefficient: " + dragCoefficient + " dragForce: " + dragForce);
        //Debug.Log("x: "+ LocalVelocity.x + " y: "+ LocalVelocity.y + " z: "+ LocalVelocity.z);
        //Debug.Log(yawForce);

        wing.MoveRotation(wing.rotation * Quaternion.Euler(new Vector3(0, -yawForce, 0) * Time.deltaTime));

        wing.AddForce(transform.up * liftForce * Time.deltaTime);
        wing.AddForce(Vector3.down * gravity * Time.deltaTime);
        wing.AddForce(transform.right * +yawForce * Time.deltaTime);
        wing.AddRelativeForce(LocalVelocity.normalized * -dragForce * Time.deltaTime);

        DrawArrow.ForDebug(transform.position,transform.up, Color.green, liftForce/20);
        DrawArrow.ForDebug(transform.position,-wing.velocity, Color.red, dragForce /20);
        DrawArrow.ForDebug(transform.position,wing.velocity, Color.blue, speed/20);
        DrawArrow.ForDebug(transform.position,transform.forward, Color.magenta, speed / 20);
        DrawArrow.ForDebug(transform.position,Vector3.down, Color.black, gravity/20);
        DrawArrow.ForDebug(transform.position, transform.right, Color.cyan, yawForce/20);

        /*
        DrawArrow.ForDebug(transform.position,transform.up, Color.green, 5f);
        DrawArrow.ForDebug(transform.position, transform.right, Color.red, 5f);
        DrawArrow.ForDebug(transform.position, transform.forward, Color.blue, 5f);
        */

        AOAMeter.SetText("alpha: " + angle.ToString());
        yawAngleMeter.SetText("Yaw Angle: " + angleYaw.ToString());
        speedoMeter.SetText("velocity: " + speed.ToString());
        heightMeter.SetText("height: " + height.ToString());

        Color color;

        if (Mathf.Abs(angle) > 15)
        {
            color = Color.red;
        }
        else if (Mathf.Abs(angle) > 1)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.white;
        }

        AOAMeter.color = color;
        yawAngleMeter.color = color;
        speedoMeter.color = color;
        heightMeter.color = color;


    }
    public void rotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}


