using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundengine : MonoBehaviour
{
    public AudioSource engineSound;

    void Update()
    {
        engineSound.volume = map01(thrustforce.thrustForce,0,100)/2;
    }
    public static float map01(float value, float min, float max)
    {
        return (value - min) * 1f / (max - min);
    }
}
