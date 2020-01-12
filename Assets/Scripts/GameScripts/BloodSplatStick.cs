using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatStick : MonoBehaviour
{
    public GameObject blood;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        blood = gameObject;
        rb = blood.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "blood")
        {
            rb.isKinematic = true;
        }

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "blood")
        {
            rb.isKinematic = true;
        }

    }
}