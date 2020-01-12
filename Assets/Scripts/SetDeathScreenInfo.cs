using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetDeathScreenInfo : MonoBehaviour {

    public int this_level;

    public GameObject round;
    public GameObject max_round;
    public GameObject kills;
    public GameObject max_kills;
    public GameObject score;
    public GameObject max_score;

    public GameObject mission1_description;
    public GameObject mission2_description;
    public GameObject mission3_description;

    public GameObject mission1_progress_bar;
    public GameObject mission2_progress_bar;
    public GameObject mission3_progress_bar;

    public GameObject xp_gain;
    public GameObject total_xp;
    public GameObject next_level_xp;
    public GameObject next_level_progress_bar;

    private GameObject master_obj;

	// Use this for initialization
    void Awake () {
        master_obj = GameObject.FindGameObjectWithTag("Master");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void refresh_death_info(){
        round.GetComponent<Text>().text = ""+Managers.game.currentRound;
        kills.GetComponent<Text>().text = "" + Managers.game.currentKills;
        score.GetComponent<Text>().text = "" + Managers.game.currentScore;
        this_level = 0;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        max_round.GetComponent<Text>().text = "" + master_obj.GetComponent<MasterStats>().maxRound[this_level];
        max_kills.GetComponent<Text>().text = "" + master_obj.GetComponent<MasterStats>().maxKills[this_level];
        max_score.GetComponent<Text>().text = "" + master_obj.GetComponent<MasterStats>().maxScore[this_level];

        Managers.missions.refresh_mission_progress();

        string[] mission1_info = Managers.missions.get_mission_info(1);
        string[] mission2_info = Managers.missions.get_mission_info(2);
        string[] mission3_info = Managers.missions.get_mission_info(3);
        mission1_description.GetComponent<Text>().text = mission1_info[0];
        mission2_description.GetComponent<Text>().text = mission2_info[0];
        mission3_description.GetComponent<Text>().text = mission3_info[0];
        mission1_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(GetFloat(mission1_info[1]),1);
        mission2_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(GetFloat(mission2_info[1]), 1);
        mission3_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(GetFloat(mission3_info[1]), 1);

        int xpGain = (int)((Managers.game.currentScore / 100f) + (50f * Managers.game.currentRound));
        xp_gain.GetComponent<Text>().text = "XP: +" +  xpGain;
        //master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().add_xp(xpGain);
        master_obj.GetComponent<MasterStats>().add_xp_finalise();
        Debug.Log("add xp  " + xpGain);
        //master_obj.GetComponent<MasterStats>().add_xp();
        total_xp.GetComponent<Text>().text = "Total xp:  " + master_obj.GetComponent<MasterStats>().get_multiplier_xp();
        int xp_of_level = (master_obj.GetComponent<MasterStats>().multiplier_xp_of_level - master_obj.GetComponent<MasterStats>().multiplier_xp_to_level);
        next_level_xp.GetComponent<Text>().text = "Next Level:  " + xp_of_level + " / " + master_obj.GetComponent<MasterStats>().multiplier_xp_of_level;
        next_level_progress_bar.GetComponent<RectTransform>().sizeDelta = new Vector2(xp_of_level * 1f / master_obj.GetComponent<MasterStats>().multiplier_xp_of_level, 1);
    }
    private float GetFloat(string stringValue)
    {
        float result = 0f;
        float.TryParse(stringValue, out result);
        return result;
    }
}
