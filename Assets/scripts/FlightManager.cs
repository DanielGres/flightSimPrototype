using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightManager : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public float initialSpeed = 10f;
    Vector3 Velocity;
    Vector3 LocalVelocity;
    Vector3 LocalAngularVelocity;
    Vector3 lastVelocity;

    float AngleOfAttack;
    float AngleOfAttackYaw;

    Vector3 LocalGForce;

    public AnimationCurve dragRight;
    public AnimationCurve dragLeft;
    public AnimationCurve dragTop;
    public AnimationCurve dragBottom;
    public AnimationCurve dragForward;
    public AnimationCurve dragBack;

    float Throttle;
    public float maxThrust;

    float inducedDrag;
    float dt;

    public AnimationCurve liftAOACurve;
    public AnimationCurve inducedDragCurve;
    public float liftPower;



    void Start()
    {

        Rigidbody = GetComponent<Rigidbody>();

        Rigidbody.velocity = Rigidbody.rotation * new Vector3(0, 0, initialSpeed);
    }

    void LateUpdate()
    {
        if (Input.GetKey("w"))
        {
            Throttle += 0.1f;
        }
        if (Input.GetKey("s"))
        {
            Throttle -= 0.1f;
        }
        Throttle = Mathf.Clamp(Throttle, 0, maxThrust);

        dt = Time.deltaTime;
        CalculateState(dt);
        CalculateGForce(dt);
        UpdateThrust();
        UpdateLift();
        UpdateDrag();

    }


    void CalculateState(float dt)
    {
        var invRotation = Quaternion.Inverse(Rigidbody.rotation);
        Velocity = Rigidbody.velocity;
        LocalVelocity = invRotation * Velocity;  //transform world velocity into local space
        LocalAngularVelocity = invRotation * Rigidbody.angularVelocity;  //transform into local space
        CalculateAngleOfAttack();
    }

    void CalculateAngleOfAttack()
    {
        AngleOfAttack = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z);
        AngleOfAttackYaw = Mathf.Atan2(LocalVelocity.x, LocalVelocity.z);
    }

    void CalculateGForce(float dt)
    {
        var invRotation = Quaternion.Inverse(Rigidbody.rotation);
        var acceleration = (Velocity - lastVelocity) / dt;
        LocalGForce = invRotation * acceleration;
        lastVelocity = Velocity;
    }

    void UpdateThrust()
    {
        Rigidbody.AddRelativeForce(Throttle * maxThrust * Vector3.forward);
    }

    void UpdateDrag()
    {
        var lv = LocalVelocity;
        var lv2 = lv.sqrMagnitude;  //velocity squared
                                    //calculate coefficient of drag depending on direction on velocity
        var coefficient = Scale6(
        lv.normalized,
        dragRight.Evaluate(Mathf.Abs(lv.x)), dragLeft.Evaluate(Mathf.Abs(lv.x)),
        dragTop.Evaluate(Mathf.Abs(lv.y)), dragBottom.Evaluate(Mathf.Abs(lv.y)),
        dragForward.Evaluate(Mathf.Abs(lv.z)),
        dragBack.Evaluate(Mathf.Abs(lv.z))
        );
    }


    Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, float liftPower, AnimationCurve aoaCurve, AnimationCurve inducedDragCurve)
    {
        var liftVelocity = Vector3.ProjectOnPlane(LocalVelocity, rightAxis);    //project velocity onto YZ plane
        var v2 = liftVelocity.sqrMagnitude;                                     //square of velocity
                                                                                //lift = velocity^2 * coefficient * liftPower
                                                                                //coefficient varies with AOA
        var liftCoefficient = aoaCurve.Evaluate(angleOfAttack * Mathf.Rad2Deg);
        var liftForce = v2 * liftCoefficient * liftPower;
        //lift is perpendicular to velocity
        var liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        var lift = liftDirection * liftForce;
        //induced drag varies with square of lift coefficient
        var dragForce = liftCoefficient * liftCoefficient;
        var dragDirection = -liftVelocity.normalized;
        Vector3 InducedDrag = dragDirection * v2 * dragForce * inducedDrag * inducedDragCurve.Evaluate(Mathf.Max(0, LocalVelocity.z));
        return lift + InducedDrag;
    }

    void UpdateLift()
    {
        float flapsLiftPower = 0; // FlapsDeployed ? this.flapsLiftPower : 0;
        float flapsAOABias = 0; // FlapsDeployed ? this.flapsAOABias : 0;
        var liftForce = CalculateLift(
            AngleOfAttack + (flapsAOABias * Mathf.Deg2Rad), Vector3.right,
            liftPower + flapsLiftPower,
            liftAOACurve,
            inducedDragCurve
        );
        //var yawForce = CalculateLift(AngleOfAttackYaw, Vector3.up, rudderPower, rudderAOACurve, rudderInducedDragCurve);
        Rigidbody.AddRelativeForce(liftForce);
        //Rigidbody.AddRelativeForce(yawForce);
    }

    Vector3 Scale6(
        Vector3 value,
        float posX, float negX,
        float posY, float negY,
        float posZ, float negZ
    )
    {
        Vector3 result = value;

        if (result.x > 0)
        {
            result.x *= posX;
        }
        else if (result.x < 0)
        {
            result.x *= negX;
        }

        if (result.y > 0)
        {
            result.y *= posY;
        }
        else if (result.y < 0)
        {
            result.y *= negY;
        }

        if (result.z > 0)
        {
            result.z *= posZ;
        }
        else if (result.z < 0)
        {
            result.z *= negZ;
        }

        return result;
    }

}
