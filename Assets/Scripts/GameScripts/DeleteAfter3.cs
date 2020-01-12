using UnityEngine;
using System.Collections;

public class DeleteAfter3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
