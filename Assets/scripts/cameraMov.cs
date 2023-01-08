using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMov : MonoBehaviour
{
    public GameObject plane;
    Vector3 pos;
    void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        transform.position = plane.transform.position + pos;
    }
}
