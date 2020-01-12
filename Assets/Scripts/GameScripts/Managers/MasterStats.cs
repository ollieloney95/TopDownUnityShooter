using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterStats : MonoBehaviour  {

    public int Mission_type1_completed=0;
    public int Mission_type2_completed=0;
    public int Mission_type3_completed=0;
    public int Mission_type4_completed=0;
    public int Mission_type5_completed=0;
    public int Mission_type6_completed=0;
    public int multiplier_level=1;
    public static int multiplier_xp;
    public int multiplier_xp_to_level = 0;
    public int multiplier_xp_of_level = 0;
    private int base_xp = 100;
    private float xp_multiplier = 1.4f;
	
    public  int[] maxRound = new int[6];
    public  int totalMaxRounds;
    public  int totalKills;
    public  int totalRounds;
    public int totalMoney;
    public  int[] maxMoney = new int[6];
    public  int[] maxScore = new int[6];
    public int[] maxKills = new int[6];
    public int[] missionTargets;
    public int[] missionProgress;
    public int[] missionIndices;
    public int maxRoundSum; 
    public int maxScoreSum; 
    GameObject manager_obj;
    private static MasterStats playerInstance;
    void Awake()
    {
        //if (FindObjectsOfType(GetType()).Length > 1)
        //{
         //   Debug.Log("destroy");
            //Destroy(FindObjectsOfType(GetType());
        //}
        if (playerInstance == null)
        {
            playerInstance = this;
            //DontDestroyOnLoad(multiplier_xp);
        }
        else
        {
            //DestroyObject(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    void update_mission_info(){
        missionTargets = Managers.missions.current_mission_targets;
        missionProgress = Managers.missions.current_mission_progress;
    }
    // Use this for initialization
	void Start () {
        //DontDestroyOnLoad(this);
        //add_xp(1);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void refresh_maxRoundSum(){
        int summ = 0;
        foreach(int i in maxRound){
            summ += i;
        }
        maxRoundSum = summ;
    }
    public void refresh_maxScoreSum()
    {
        int summ = 0;
        foreach (int i in maxScore)
        {
            summ += i;
        }
        maxScoreSum = summ;
    }

    public void add_xp_from_death(int xp)
    {
        multiplier_xp += xp;
        Managers.missions.set_multiplier_xp(multiplier_xp);
        int new_level = 1;
        new_level = 1+ (int)Mathf.Floor(Mathf.Log(1f + (xp_multiplier - 1f) * multiplier_xp / base_xp, xp_multiplier));
        if (new_level > multiplier_level)
        {
            multiplier_level = new_level;
        }
        add_xp_finalise();
    }
    public int get_multiplier_xp(){
        return(multiplier_xp);
    }
    public void set_multiplier_xp(int xp)
    {
        multiplier_xp = xp;
        return;
    }
    public void add_xp(int xp){
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            //set gamemode to xp screen

            manager_obj = GameObject.FindGameObjectWithTag("Manager");
            manager_obj.GetComponent<MainMenuUIController>().missionState = "XPScreen";
            manager_obj.GetComponent<MainMenuUIController>().buttonMissionScreen();


            GameObject.FindGameObjectWithTag("XP_screen").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("XP_screen").transform.GetChild(0).gameObject.GetComponent<SetXPScreen>().set_screen(xp);


        }
        multiplier_xp += xp;
        Managers.missions.set_multiplier_xp(multiplier_xp);
        Debug.Log("actually added this xp   " + multiplier_xp);

    }
    public void add_xp_finalise(){
        int new_level = 0;
        new_level = 1 + (int)Mathf.Floor(Mathf.Log(1f + (xp_multiplier - 1f) * multiplier_xp / base_xp, xp_multiplier));
        if (new_level > multiplier_level)
        {
            multiplier_level = new_level;
        }
        refresh_level_xp_info();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {

            manager_obj = GameObject.FindGameObjectWithTag("Manager");
            manager_obj.GetComponent<MainMenuUIController>().refreshTextMultiplier();
            //then set game mode back to mission at end 
            manager_obj.GetComponent<MainMenuUIController>().missionState = "main";
            manager_obj.GetComponent<MainMenuUIController>().buttonMissionScreen();
        }
    }
    public void refresh_level_xp_info(){
        int total_xp_of_next_level;
        int total_xp_of_this_level;
        total_xp_of_next_level = (int)(base_xp * ((1f -Mathf.Pow(xp_multiplier, multiplier_level)) / (1f - xp_multiplier)));
        total_xp_of_this_level = (int)(base_xp * ((1f - Mathf.Pow(xp_multiplier, multiplier_level -1f )) / (1f - xp_multiplier)));
        multiplier_xp_of_level = total_xp_of_next_level - total_xp_of_this_level;
        multiplier_xp_to_level = total_xp_of_next_level - multiplier_xp;
    }
}
