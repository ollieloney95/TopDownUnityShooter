using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateshop : MonoBehaviour
{
    public GameObject lighting;
    // Use this for initialization
    void Start()
    {
        //lighting = this.transform.Find("light").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -30 * Time.deltaTime);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.Rotate(0,0, 180 * Time.deltaTime);
            lighting.GetComponent<Light>().color = Color.green;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            lighting.GetComponent<Light>().color = Color.red;
        }
    }
}
