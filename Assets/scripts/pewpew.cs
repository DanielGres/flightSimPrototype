using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pewpew : MonoBehaviour
{
    public GameObject bullet;

    public float firerate = 0.1f;

    float nextFire = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + firerate;
            Instantiate(bullet, transform.position + transform.forward * 2.2f, transform.rotation);
        }
    }
}
