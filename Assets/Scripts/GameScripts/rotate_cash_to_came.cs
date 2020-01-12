using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_cash_to_came : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(180, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(180* Vector3.up * Time.deltaTime);
	}
}
