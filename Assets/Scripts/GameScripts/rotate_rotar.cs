using UnityEngine;
using System.Collections;

public class rotate_rotar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0,720 * Time.deltaTime);
        
	}
}
