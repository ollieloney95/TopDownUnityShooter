using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAim : MonoBehaviour {
    public float dist;
    public GameObject line;
    public LayerMask layerMask;
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        
	}
    public void draw_aim(){
        line.GetComponent<LineRenderer>().enabled = true;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, layerMask))
        {
            dist = hit.distance;
        }
        else
        {
            dist = 20f;
        }
        line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, dist * 3));
        //Debug.DrawRay(transform.position, transform.forward * dist, Color.green);
    }
    public void hide_aim()
    {
        line.GetComponent<LineRenderer>().enabled = false;
    }
}
