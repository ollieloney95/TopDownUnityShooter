using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetXPScreen : MonoBehaviour {

    public int xp_gain;

    public GameObject total_xp;
    public GameObject xp_gain_obj;
    public GameObject next_level_xp;
    public GameObject next_level_progress_bar;
    private GameObject master_obj;

	// Use this for initialization
    void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void set_screen(int xp_gain_var){
        xp_gain = xp_gain_var;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().refresh_level_xp_info();
        int xp_of_level = master_obj.GetComponent<MasterStats>().multiplier_xp_of_level;
        int xp_earned_this_level = master_obj.GetComponent<MasterStats>().multiplier_xp_of_level - master_obj.GetComponent<MasterStats>().multiplier_xp_to_level;

        total_xp.GetComponent<Text>().text = "Total XP:  " + master_obj.GetComponent<MasterStats>().get_multiplier_xp();
        xp_gain_obj.GetComponent<Text>().text = "XP gain:  " + xp_gain;
        next_level_xp.GetComponent<Text>().text = "" + xp_earned_this_level + "  /  " + xp_of_level;

        next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2( Mathf.Min(1f,(xp_gain + xp_earned_this_level) * 1f) / xp_of_level, 5f);
        Debug.Log("about to start coroutine");
        StartCoroutine(add_xp(xp_gain_var,xp_of_level,xp_earned_this_level));
    }

    IEnumerator add_xp(int xp_gain_var,int xp_of_level,int xp_earned_this_level)
    {
        Time.timeScale = 1f;
        Debug.Log("Time.timeScale " +Time.timeScale);
        //Debug.Log("Started add xp coroutine");
        yield return new WaitForSeconds(0.5f);
        float time_counter = 0;
        float xp_add;
        xp_add = 0f;
        //Debug.Log("starting coroutine");
        //Debug.Log(" Time.deltaTime  " + Time.deltaTime);
        Debug.Log("xp_of_level + " + xp_of_level);
        Debug.Log("xp_earned_this_level + " + xp_earned_this_level);
        while (time_counter < 2f)
        {
            //Debug.Log("time_counter  " + time_counter);
            float cap_value_modifier = Mathf.Min(1f,   (xp_of_level-xp_earned_this_level)*1f / (xp_gain_var));
            Debug.Log("cap_value_modifier + " + cap_value_modifier);
            xp_add = (xp_gain_var * (time_counter) / 2f) * cap_value_modifier;
            if (xp_earned_this_level + xp_add < xp_of_level)
            {
                next_level_xp.GetComponent<Text>().text = "" + (xp_earned_this_level + (int)(xp_add)) + "  /  " + xp_of_level;
                next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Min(1f,(xp_earned_this_level+xp_add * 1f) / xp_of_level), 5f);
                Debug.Log("(xp_earned_this_level+xp_add * 1f) / xp_of_level   " + (xp_earned_this_level + xp_add * 1f) / xp_of_level);
                time_counter += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }else{
                next_level_xp.GetComponent<Text>().text = " NEXT LEVEL!";
                next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 5f);
                Debug.Log("first one");
                //flash animation
                //yield return new WaitForSeconds(0.02f);
                time_counter = 2f;
            }
        }
        if (time_counter != 2f){
            time_counter = 2f;
            xp_add = (xp_gain_var * (time_counter) / 2f);
            if (xp_earned_this_level + xp_add < xp_of_level)
            {
                next_level_xp.GetComponent<Text>().text = "" + (xp_earned_this_level + (int)(xp_add)) + "  /  " + xp_of_level;
                next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Min(1f, (xp_earned_this_level + xp_add * 1f) / xp_of_level), 5f);
            }
            else
            {
                next_level_xp.GetComponent<Text>().text = " NEXT LEVEL!";
                next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 5f);
                time_counter = 2f;
                Debug.Log("second one");
            }
        }
        //Debug.Log("FInished coroutine");

        yield return new WaitForSeconds(1f);
        master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().add_xp_finalise();

    }
}
