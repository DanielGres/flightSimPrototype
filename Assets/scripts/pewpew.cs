using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pewpew : MonoBehaviour
{
    public GameObject bullet;

    public float firerate = 0.1f;

    float nextFire = 0f;

    public float position = 3f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + firerate;
            Instantiate(bullet, transform.position + transform.forward * position, transform.rotation);
        }
    }
}
