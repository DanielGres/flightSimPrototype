using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwap : MonoBehaviour
{
    public Camera[] cam;
    int counter;

    void Start()
    {
        cam[0].enabled = true;
        counter = 1;
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            cam[counter].enabled = false;
            counter++;
            if (counter == cam.Length) { counter = 0; }
            cam[counter].enabled = true;
        }
    }
}
