using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private int multiplier_level;
    private int multiplier_xp;
    public GameObject[] progress_bars;
    public GameObject[] Mission_descriptions;
    public GameObject[] Mission_backgrounds;
    public GameObject[] Mission_titles;
    public Sprite[] mission_background_sprites;

    private int[] Mission_stages_xp = new int[] { 100, 500, 1000, 5000 };
    public int[] Missions_completed = new int[] {0,0,0,0,0 ,0};

    public void set_multiplier_xp(int xp){
        multiplier_xp = xp;
        return;
    }
    //  total kill stages     index 1
    private int[] Mission_type1_stages = new int[]{10, 50, 100, 200};
    //private int Mission_type1_completed;


    //  total levels stages    index 2
    private int[] Mission_type2_stages = new int[] { 5, 10, 20, 50 };
    //private int Mission_type2_completed;


    //  maximum round stages    index 3
    private int[] Mission_type3_stages = new int[] { 2, 4, 6, 8 };
    //private int Mission_type3_completed;


    //  max score stages    index 4
    private int[] Mission_type4_stages = new int[] { 50, 100, 200, 500 };
    //private int Mission_type4_completed;


    //  max kills stages    index 5
    private int[] Mission_type5_stages = new int[] { 20, 40, 100, 500 };
    //private int Mission_type5_completed;


    //  max money stages    index 6
    private int[] Mission_type6_stages = new int[] { 200, 400, 600, 800 };
    //private int Mission_type6_completed;


    //  current missions
    public int[] current_mission_indices = new int[] { 1, 4, 2 };
    public int[] current_mission_targets;
    public int[] current_mission_progress = new int[] { 1, 1, 1 };
    public bool[] mission_complete = new bool[] { false, false, false };

    void Awake()
    {
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");

        Missions_completed[0] = master_obj.GetComponent<MasterStats>().Mission_type1_completed;
        Missions_completed[1] = master_obj.GetComponent<MasterStats>().Mission_type2_completed;
        Missions_completed[2] = master_obj.GetComponent<MasterStats>().Mission_type3_completed;
        Missions_completed[3] = master_obj.GetComponent<MasterStats>().Mission_type4_completed;
        Missions_completed[4] = master_obj.GetComponent<MasterStats>().Mission_type5_completed;
        Missions_completed[5] =  master_obj.GetComponent<MasterStats>().Mission_type6_completed;
        multiplier_level = master_obj.GetComponent<MasterStats>().multiplier_level;
        multiplier_xp = master_obj.GetComponent<MasterStats>().get_multiplier_xp();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            refresh_mission_targets();
            refresh_missions();
        }else{
            refresh_mission_targets();
            refresh_mission_progress();
        }
    }
    public void refresh_mission_progress(){
        int i = 0;
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        for (i = 0; i < 3; i++)
        {
            if (current_mission_indices[i] == 1) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().totalKills; }
            else if (current_mission_indices[i] == 2) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().totalRounds; }
            else if (current_mission_indices[i] == 3) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxRound[0]; }
            else if (current_mission_indices[i] == 4) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxScore[0]; }
            else if (current_mission_indices[i] == 5) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxKills[0]; }
            else if (current_mission_indices[i] == 6) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxMoney[0]; }
        }
        master_obj.GetComponent<MasterStats>().missionTargets = current_mission_targets;
        master_obj.GetComponent<MasterStats>().missionProgress = current_mission_progress;
        master_obj.GetComponent<MasterStats>().missionIndices = current_mission_indices;
    }
    public void refresh_missions(){
        int i = 0;
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        for (i = 0; i < 3; i++)
        {
            if (current_mission_indices[i] == 1) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().totalKills; }
            else if (current_mission_indices[i] == 2) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().totalRounds; }
            else if (current_mission_indices[i] == 3) { current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxRound[0]; }
            else if (current_mission_indices[i] == 4){ current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxScore[0]; }
            else if (current_mission_indices[i] == 5){ current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxKills[0]; }
            else if (current_mission_indices[i] == 6){ current_mission_progress[i] = master_obj.GetComponent<MasterStats>().maxMoney[0]; }

            if(current_mission_progress[i] >= current_mission_targets[i]){
                current_mission_progress[i] = current_mission_targets[i];
                mission_complete[i] = true;
                Mission_backgrounds[i].GetComponent<Image>().sprite = mission_background_sprites[1];
            }else{
                Mission_backgrounds[i].GetComponent<Image>().sprite = mission_background_sprites[0];
            }

            progress_bars[i].GetComponent<RectTransform>().sizeDelta = new Vector2(current_mission_progress[i] *1f /current_mission_targets[i],progress_bars[i].GetComponent<RectTransform>().sizeDelta.y);
            progress_bars[3+i].GetComponent<RectTransform>().sizeDelta = new Vector2(current_mission_progress[i] * 1f / current_mission_targets[i], progress_bars[i+3].GetComponent<RectTransform>().sizeDelta.y);
        }
        refresh_mission_text();
        master_obj.GetComponent<MasterStats>().missionTargets = current_mission_targets;
        master_obj.GetComponent<MasterStats>().missionProgress = current_mission_progress;
        master_obj.GetComponent<MasterStats>().missionIndices = current_mission_indices;

    }
    void refresh_mission_text(){
        int i = 0;
        for (i = 0; i < 3; i++)
        {
            if (current_mission_indices[i] == 1) {  Mission_descriptions[i].GetComponent<Text>().text = "Reach a total of "+ current_mission_targets[i] +" kills";}
            else if (current_mission_indices[i] == 2) { Mission_descriptions[i].GetComponent<Text>().text = "Reach a total of " + current_mission_targets[i] +" rounds passed"; }
            else if (current_mission_indices[i] == 3) { Mission_descriptions[i].GetComponent<Text>().text = "Reach a maximum round of " + current_mission_targets[i]; }
            else if (current_mission_indices[i] == 4) { Mission_descriptions[i].GetComponent<Text>().text = "Reach a maximum score of " + current_mission_targets[i]; }
            else if (current_mission_indices[i] == 5) { Mission_descriptions[i].GetComponent<Text>().text = "Reach a maximum kill count of " + current_mission_targets[i]; }
            else if (current_mission_indices[i] == 6) { Mission_descriptions[i].GetComponent<Text>().text = "Reach a maximum cash count of " + current_mission_targets[i]; }
        }
    }
    public void refresh_mission_targets(){
        current_mission_targets = new int[] {0,0,0};
        int i = 0;
        for (i = 0; i < 3; i++)
        {
            if (current_mission_indices[i] == 1) { current_mission_targets[i] = Mission_type1_stages[Missions_completed[0]]; }
            else if (current_mission_indices[i] == 2) { current_mission_targets[i] = Mission_type2_stages[Missions_completed[1]]; }
            else if (current_mission_indices[i] == 3) { current_mission_targets[i] = Mission_type3_stages[Missions_completed[2]]; }
            else if (current_mission_indices[i] == 4) { current_mission_targets[i] = Mission_type4_stages[Missions_completed[3]]; }
            else if (current_mission_indices[i] == 5) { current_mission_targets[i] = Mission_type5_stages[Missions_completed[4]]; }
            else if (current_mission_indices[i] == 6) { current_mission_targets[i] = Mission_type6_stages[Missions_completed[5]]; }

        }
    }
    public void add_xp(int xp)
    {
        multiplier_xp += xp;
    }
    void OnDestroy()
    {
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");

        master_obj.GetComponent<MasterStats>().Mission_type1_completed = Missions_completed[0];
        master_obj.GetComponent<MasterStats>().Mission_type2_completed = Missions_completed[1];
        master_obj.GetComponent<MasterStats>().Mission_type3_completed = Missions_completed[2];
        master_obj.GetComponent<MasterStats>().Mission_type4_completed = Missions_completed[3];
        master_obj.GetComponent<MasterStats>().Mission_type5_completed = Missions_completed[4];
        master_obj.GetComponent<MasterStats>().Mission_type6_completed = Missions_completed[5];
        //master_obj.GetComponent<MasterStats>().multiplier_level = multiplier_level;
        master_obj.GetComponent<MasterStats>().set_multiplier_xp(multiplier_xp);
    }
    public void Startup()
    {
        Debug.Log("Mission manager starting...");
        status = ManagerStatus.Started;
    }
    void Update()
    {
        //refresh_missions();
    }
    public void update_mission_info(){
        int mission_number = int.Parse(gameObject.GetComponent<MainMenuUIController>().missionState.Substring(7));
        string progress = "ewd";
        string target = "ewd";
        string percent = "ewd";
        string xp = "ewd";
        string difficulty = "ewd";
        int mission_type = current_mission_indices[mission_number-1];
        if(mission_type == 1){    //total kills
            progress = " " + current_mission_progress[mission_number-1]+ " kills";
            target = " " + current_mission_targets[mission_number-1] +" kills";
            percent = " " + current_mission_progress[mission_number- 1] / (0.01*current_mission_targets[mission_number- 1]) +"%";
            xp = " " + Mission_stages_xp[Missions_completed[0]]+ " xp";
            difficulty = " " + Missions_completed[0]+" /5";
        }
        if (mission_type == 2) //total round
        {
            progress = " " + current_mission_progress[mission_number- 1]+" rounds";
            target = " " + current_mission_targets[mission_number- 1] +" rounds";
            percent = " " + current_mission_progress[mission_number- 1] / (0.01 * current_mission_targets[mission_number- 1])+"%";
            xp = " " + Mission_stages_xp[Missions_completed[1]]+" xp";
            difficulty = " " +Missions_completed[1]+" /5";
        }
        if (mission_type == 3) //max round
        {
            progress = " " + current_mission_progress[mission_number- 1]+" rounds";
            target = " " + current_mission_targets[mission_number- 1] +" rounds";
            percent = " " +current_mission_progress[mission_number- 1] / (0.01 * current_mission_targets[mission_number- 1]) +"%";
            xp = " " + Mission_stages_xp[Missions_completed[2]]+" xp";
            difficulty = " " + Missions_completed[2]+" /5";
        }
        if (mission_type == 4) //max score
        {
            progress = " " + current_mission_progress[mission_number- 1]+" score";
            target = " " + current_mission_targets[mission_number- 1] +" score";
            percent = " " + current_mission_progress[mission_number- 1] / (0.01 * current_mission_targets[mission_number- 1])+"%";
            xp = " " + Mission_stages_xp[Missions_completed[3]]+" xp";
            difficulty = " " + Missions_completed[3]+" /5";
        }
        if (mission_type == 5)  //max kill
        {
            progress = " " + current_mission_progress[mission_number - 1] + " kills";
            target = " " + current_mission_targets[mission_number - 1] + " kills";
            percent = " " + current_mission_progress[mission_number - 1] / (0.01 * current_mission_targets[mission_number - 1]) + "%";
            xp = " " + Mission_stages_xp[Missions_completed[4]] + " xp";
            difficulty = " " + Missions_completed[4] + " /5";
        }
        if (mission_type == 6)  //max money
        {
            progress = " " + current_mission_progress[mission_number- 1]+" cash";
            target = " " + current_mission_targets[mission_number- 1] +" cash";
            percent = " " +current_mission_progress[mission_number- 1] / (0.01 * current_mission_targets[mission_number- 1]) +"%";
            xp = " " + Mission_stages_xp[Missions_completed[5]]+" xp";
            difficulty = " " + Missions_completed[5] +" /5";
        }
        gameObject.GetComponent<MainMenuUIController>().mission_info_setup(progress,  target,  percent,  xp,  difficulty,mission_number);
    }

    public void complete_mission(int mission_id){
        int mission_type = current_mission_indices[mission_id-1];
        mission_complete[mission_id - 1] = false;
        multiplier_xp = Mission_stages_xp[Missions_completed[mission_type-1]];
        Missions_completed[mission_type - 1]++;
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");

        if (mission_type == 1) { master_obj.GetComponent<MasterStats>().Mission_type1_completed++; }
        if (mission_type == 2) { master_obj.GetComponent<MasterStats>().Mission_type2_completed++; }
        if (mission_type == 3) { master_obj.GetComponent<MasterStats>().Mission_type3_completed++; }
        if (mission_type == 4) { master_obj.GetComponent<MasterStats>().Mission_type4_completed++; }
        if (mission_type == 5) { master_obj.GetComponent<MasterStats>().Mission_type5_completed++; }
        if (mission_type == 6) { master_obj.GetComponent<MasterStats>().Mission_type6_completed++; }
        int new_mission_type = Random.Range(1, 7);
        while(current_mission_indices[0] == new_mission_type || new_mission_type == current_mission_indices[1] || new_mission_type == current_mission_indices[2]){
            new_mission_type = Random.Range(1, 7);
        }
        current_mission_indices[mission_id-1] = new_mission_type;
        refresh_mission_targets();
        //master_obj.GetComponent<MasterStats>().add_xp(Mission_stages_xp[Missions_completed[mission_type - 1]]);
        master_obj.GetComponent<MasterStats>().add_xp(multiplier_xp);
        //refresh mission progress and mission targets 
        refresh_missions();


    }
    public bool is_mission_complete(int id){
        if(current_mission_progress[id-1]>=current_mission_targets[id-1]){
            return(true);
        }
        return (false);
    }


    public string[] get_mission_info(int mission_number)    //return mission info as strings,  {mission description, mission progress (ie 0.23)}
    {
        string mission_description = "description";
        string mission_progress = "0.11";
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        int mission_type = master_obj.GetComponent<MasterStats>().missionIndices[mission_number - 1];

        if (mission_type == 1)
        {    //total kills
            //mission_description = "Reach a total of " + current_mission_progress[mission_number - 1] + " kills";

            mission_description = "Reach a total of " + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " kills";
            mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        if (mission_type == 2) //total round
        {
            mission_description = "Reach a total of " + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " rounds passed";
                mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        if (mission_type == 3) //max round
        {
            mission_description = "Reach round" + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " on any map";
                mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        if (mission_type == 4) //max score
        {
            mission_description = "Reach a score of " + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " on any map";
                    mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        if (mission_type == 5)  //max kill
        {
            mission_description = "Reach a killcount of " + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " on any map";
                        mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        if (mission_type == 6)  //max money
        {
            mission_description = "collect " + master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1] + " dollars on any map";
                            mission_progress = "" + Mathf.Min(1f,(master_obj.GetComponent<MasterStats>().missionProgress[mission_number - 1] / (1f * master_obj.GetComponent<MasterStats>().missionTargets[mission_number - 1])));
        }
        return(new string[] { mission_description, mission_progress });
    }


}