using UnityEngine;
using System.Collections;

public class SplatMovement : MonoBehaviour {
    public Rigidbody blood;
	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(8,8);
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f,1f), Random.Range(1f, 2f), Random.Range(-1f, 1f));
        Destroy(gameObject, 3);

    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, gameObject.GetComponent<Renderer>().material.color.a-0.01f);

    }
}
