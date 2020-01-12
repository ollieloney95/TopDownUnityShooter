using UnityEngine;
using System.Collections;

public class SplatDirt : MonoBehaviour {
    //public Rigidbody blood;
	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(8,8);
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f,1f), Random.Range(2f, 4f), Random.Range(-1f, 1f));
        Destroy(gameObject, 3);
        //StartCoroutine(fade());

    }
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(fade());
        //gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, gameObject.GetComponent<Renderer>().material.color.a-0.001f);

    }

    //public IEnumerator fade()
    //{
        //yield return new WaitForSeconds(1);
        //Debug.Log("fade");
        //while (gameObject.GetComponent<Renderer>().material.color.a>0f) {
        //Debug.Log("fading");
            //gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        //}
    //}
}
