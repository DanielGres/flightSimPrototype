using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float timeDeath = 10f;
    public float force = 100f;
    public AudioSource audioD;
    void Start()
    {
        audioD.Play();
        Rigidbody bulletRB = transform.GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * force);
        Destroy(gameObject, timeDeath);
    }

}
