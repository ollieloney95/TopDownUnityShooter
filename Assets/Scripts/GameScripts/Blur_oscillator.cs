using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Blur_oscillator : MonoBehaviour {

    private BlurOptimized blurr;
    private float blur_value = 0f;
    public float blur_speed=1f;
    public float max_blur = 4f;
    private bool blur_direction = true;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        blurr = this.gameObject.GetComponent<BlurOptimized>();
        if(blur_direction == true)
        {
            blurr.blurSize += blur_speed*Time.deltaTime;
        }
        else
        {
            blurr.blurSize -= blur_speed* Time.deltaTime;
        }
        if (blurr.blurSize < 0f)
        {
            blur_direction = true;
            blurr.blurSize = 0f;
        }
        if (blurr.blurSize > max_blur)
        {
            blur_direction = false;
            blurr.blurSize = max_blur;
        }

    }
}
