using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hardcoreAngles : MonoBehaviour
{
    public GameObject AircraftPoint;

    float xDif;
    float yDif;
    void Start()
    {
        
    }

    void Update()
    {
        float angleR = Vector2.SignedAngle(new Vector2(transform.position.x - AircraftPoint.transform.position.x, transform.position.y - AircraftPoint.transform.position.y),Vector2.one);
        float angleP = Vector2.SignedAngle(new Vector2(transform.position.y - AircraftPoint.transform.localPosition.y, transform.position.z - AircraftPoint.transform.localPosition.z), Vector2.up);
        float angleY = Vector2.SignedAngle(new Vector2(transform.position.z - AircraftPoint.transform.localPosition.z, transform.position.x - AircraftPoint.transform.localPosition.x), Vector2.right);

        Debug.Log("Roll: "+angleR+" Pitch: "+angleP+" Yaw: "+angleY);

        //yDif = AngleDifference(transform.rotation.eulerAngles.y ,AircraftPoint.transform.rotation.eulerAngles.y);
        //xDif = AngleDifference(AircraftPoint.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.x);

    }

    float AngleDifference(float a, float b)
    {
        return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
    }
}
