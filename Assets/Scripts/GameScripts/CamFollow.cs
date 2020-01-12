using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

    [SerializeField] public Transform player;
    [SerializeField] public Transform cameraPosition;
    [SerializeField] public float sensitivity = 0.1f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, cameraPosition.position);
        if (distance < 0.1)
        {
            
        }
        //transform.position = Vector3.Lerp(transform.position, cameraPosition.position, sensitivity);
        //transform.rotation = cameraPosition.rotation;
        //transform.Rotate(90, 0, 0);
    }
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, sensitivity);
        transform.rotation = cameraPosition.rotation;
        transform.Rotate(90, 0, 0);
    }
}
