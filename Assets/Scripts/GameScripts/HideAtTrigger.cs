using UnityEngine;
using System.Collections;

public class HideAtTrigger : MonoBehaviour {
    [SerializeField]
    public GameObject trigger;
    [SerializeField]
    public GameObject[] toHide;
    [SerializeField]
    public Material[] mat;
    [SerializeField] public Material hideMat;
    // Use this for initialization
    void Start () {
        mat = new Material[toHide.Length];
        int i = 0;
        foreach (GameObject obj in toHide)
        {
            mat[i] = obj.GetComponent<MeshRenderer>().material;
            i++;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            int i = 0;
            foreach (GameObject obj in toHide)
            {
                
                //Color color = obj.GetComponent<MeshRenderer>().material.color;
                //color.a = 0.1f;
                //obj.GetComponent<MeshRenderer>().material.color = color;
                obj.GetComponent<MeshRenderer>().material = hideMat;
                i++;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            int i = 0;
            foreach (GameObject obj in toHide)
            {

                //Color color = obj.GetComponent<MeshRenderer>().material.color;
                //color.a = 0.1f;
                //obj.GetComponent<MeshRenderer>().material.color = color;
                obj.GetComponent<MeshRenderer>().material = hideMat;
                i++;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            int i = 0;
            foreach (GameObject obj in toHide)
            {
                //Color color = obj.GetComponent<MeshRenderer>().material.color;
                //color.a = 0.1f;
                //obj.GetComponent<MeshRenderer>().material.color = color;
                obj.GetComponent<MeshRenderer>().material = (Material)mat[i];
                i++;
            }
        }
    }

}
