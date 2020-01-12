using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade_enterence : MonoBehaviour {


    public float fade_time = 5f;
    public GameObject fade_UI;
    float timer = 0f;
    Color c;
    Image image;
	// Use this for initialization
	void Start () {
        image = fade_UI.GetComponent<Image>();
        c = image.color;
        image.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < fade_time){
            c.a-=Time.deltaTime / fade_time;
            image.color = c;
        }else{
            Destroy(fade_UI);
        }
        timer += Time.deltaTime;

	}
}
