using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwap : MonoBehaviour
{
    public Camera[] cam;
    public GameObject[] test;
    int counter;

    void Start()
    {
        cam[0].enabled = true;
        test[1].SetActive(false);
        test[2].SetActive(false);
        test[3].SetActive(false);
        counter = 1;
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            cam[counter].enabled = false;
            test[counter].SetActive(false);
            counter++;
            if (counter == cam.Length) { counter = 0; }
            test[counter].SetActive(true);
            cam[counter].enabled = true;
        }
    }
}
