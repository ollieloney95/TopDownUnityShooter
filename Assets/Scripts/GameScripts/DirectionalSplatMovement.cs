using UnityEngine;
using System.Collections;

public class DirectionalSplatMovement : MonoBehaviour {
    //public Rigidbody blood;
	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(8,8);
        Physics.IgnoreLayerCollision(8, 10);
        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(2f,4f), Random.Range(1f, 2f), Random.Range(-2f, 2f));
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(Random.Range(-200f, 200f), Random.Range(100f, 200f), Random.Range(200f, 400f)); 
        Destroy(gameObject, 3);

    }
	
	// Update is called once per  frame
	void Update () {
        //gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, gameObject.GetComponent<Renderer>().material.color.a-0.01f);

    }
}
