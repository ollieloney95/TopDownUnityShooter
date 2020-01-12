using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{

    [SerializeField] private Text multiplierText;
    [SerializeField] private Text totalRoundText;
    [SerializeField] private Image multiplierImage;
    public GameObject multiplier_xp_bar_front;
    public GameObject multiplier_xp_bar_text;
    public GameObject multiplier_xp_bar_front_2;
    public GameObject multiplier_xp_bar_text_2;
    public GameObject scoreText;
    public GameObject[] maxScoress;
    public GameObject[] maxRounds;


    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;
    [SerializeField] private GameObject level4;
    [SerializeField] private GameObject level5;
    [SerializeField] private GameObject level6;
    [SerializeField] private GameObject levelCollection;
    [SerializeField] private GameObject missionList;
    [SerializeField] private GameObject hordeMode;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject mute;
    [SerializeField] private GameObject leaderBoards;
    [SerializeField] private GameObject leaderboardScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject missionScreen;
    [SerializeField] private GameObject missionScreenInfo;
    [SerializeField] private GameObject mission_info_button;
    [SerializeField] private GameObject xpScreen;


    [SerializeField] public string gameState;
    [SerializeField] private ArrayList gameStates;
    [SerializeField] public string missionState;
    [SerializeField] private ArrayList missionStates;
    [SerializeField] private Text[] levelMaxRounds;
    [SerializeField] private Sprite[] levelsUnlocked;
    [SerializeField] private Sprite[] levelsLocked;
    public GameObject[] missionScreenInfoObjects;

    void setUpgameStates()
    {
        gameStates.Add("Main");
        gameStates.Add("Settings");
        gameStates.Add("Leaderboard");
        gameStates.Add("Mission");
        gameStates.Add("HordeMode");
        gameStates.Add("XPScreen");

        missionStates.Add("main");
        missionStates.Add("mission1");
        missionStates.Add("mission2");
        missionStates.Add("mission3");
    }
    void refreshMaxRoundsAndScores(){
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master"); 

        maxScoress[0].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[0];  
        maxScoress[1].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[1];  
        maxScoress[2].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[2];  
        maxScoress[3].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[3];  
        maxScoress[4].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[4];  
        maxScoress[5].GetComponent<Text>().text = "Score:  "+ master_obj.GetComponent<MasterStats>().maxScore[5];
        Debug.Log("max score  " + master_obj.GetComponent<MasterStats>().maxScore[0]);

        maxRounds[0].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[0];    
        maxRounds[1].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[1];  
        maxRounds[2].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[2];  
        maxRounds[3].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[3];  
        maxRounds[4].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[4];  
        maxRounds[5].GetComponent<Text>().text = "Round:  "+ master_obj.GetComponent<MasterStats>().maxRound[5];  
    }
    void refreshButtons()
    {
        //refreshTextMultiplier();
        refreshTextMultiplier();
        buttonLevelRefresh();
        buttonMissionList();
        buttonSettings();
        buttonBack();
        buttonMute();
        buttonHordeMode();
        buttonLeaderboards();
        buttonLeaderboardScreen();
        buttonSettingsScreen();
        buttonMissionScreen();
        setTotalRoundText();
        refreshLevelUIs();
        refreshScore();
        refreshMaxRoundsAndScores();
        //make back button
    }
    public void refreshTextMultiplier()
    {
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().refresh_level_xp_info();
        Managers.game.currentMultiplier = master_obj.GetComponent<MasterStats>().multiplier_level;
        multiplierText.text = "Multiplier: x" + Managers.game.currentMultiplier;

        int multiplier_xp_to_level = master_obj.GetComponent<MasterStats>().multiplier_xp_to_level;
        int multiplier_xp_of_level = master_obj.GetComponent<MasterStats>().multiplier_xp_of_level;

        multiplier_xp_bar_front.GetComponent<RectTransform>().sizeDelta = new Vector2(((multiplier_xp_of_level - multiplier_xp_to_level) * 1.0f) / multiplier_xp_of_level, 1f);
        multiplier_xp_bar_front_2.GetComponent<RectTransform>().sizeDelta = new Vector2(((multiplier_xp_of_level - multiplier_xp_to_level) * 1.0f) / multiplier_xp_of_level, 1f);
        multiplier_xp_bar_text.GetComponent<Text>().text = (multiplier_xp_of_level - multiplier_xp_to_level) + "  /  " + multiplier_xp_of_level + "    xp to next level";
        multiplier_xp_bar_text_2.GetComponent<Text>().text = (multiplier_xp_of_level - multiplier_xp_to_level) + "  /  " + multiplier_xp_of_level + "    xp to next level";
    }
    public void refreshScore()
    {
        int score;
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().refresh_maxScoreSum();
        score = master_obj.GetComponent<MasterStats>().maxScoreSum;
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }
    void buttonLevelRefresh()
    {
        if (gameState == "Main") {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
            level5.SetActive(false);
            level6.SetActive(false);   ///repeat for rest 
            levelCollection.SetActive(false);
        }
        if (gameState == "Settings") {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
            level5.SetActive(false);
            level6.SetActive(false);
            levelCollection.SetActive(false);
        }
        if (gameState == "Leaderboard") {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
            level5.SetActive(false);
            level6.SetActive(false);
            levelCollection.SetActive(false);
        }
        if (gameState == "Mission") {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
            level5.SetActive(false);
            level6.SetActive(false);
            levelCollection.SetActive(false);
        }
        if (gameState == "HordeMode") {
            level1.SetActive(true);
            level2.SetActive(true);
            level3.SetActive(true);
            level4.SetActive(true);
            level5.SetActive(true);
            level6.SetActive(true);
            levelCollection.SetActive(true);
        }
        if (gameState == "XPScreen")
        {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
            level5.SetActive(false);
            level6.SetActive(false);
            levelCollection.SetActive(false);
        }
    }
    void buttonMissionList()
    {
        if (gameState == "Main") { missionList.SetActive(true); }
        if (gameState == "Settings") { missionList.SetActive(false); }
        if (gameState == "Leaderboard") { missionList.SetActive(false); }
        if (gameState == "Mission") { missionList.SetActive(false); }
        if (gameState == "HordeMode") { missionList.SetActive(false); }


    }
    void buttonSettings()
    {
        if (gameState == "Main") { settings.SetActive(true); }
        if (gameState == "Settings") { settings.SetActive(true); }
        if (gameState == "Leaderboard") { settings.SetActive(false); }
        if (gameState == "Mission") { settings.SetActive(false); }
        if (gameState == "HordeMode") { settings.SetActive(false); }
    }
    void buttonBack()
    {
        if (gameState == "Main") { back.SetActive(false); }
        if (gameState == "Settings") { back.SetActive(true); }
        if (gameState == "Leaderboard") { back.SetActive(true); }
        if (gameState == "Mission") { back.SetActive(true); }
        if (gameState == "HordeMode") { back.SetActive(true); }
    }
    void buttonMute()
    {
        if (gameState == "Main") { mute.SetActive(true); }
        if (gameState == "Settings") { mute.SetActive(false); }
        if (gameState == "Leaderboard") { mute.SetActive(false); }
        if (gameState == "Mission") { mute.SetActive(false); }
        if (gameState == "HordeMode") { mute.SetActive(false); }
    }
    void buttonHordeMode()
    {
        if (gameState == "Main") { hordeMode.SetActive(true); }
        if (gameState == "Settings") { hordeMode.SetActive(false); }
        if (gameState == "Leaderboard") { hordeMode.SetActive(false); }
        if (gameState == "Mission") { hordeMode.SetActive(false); }
        if (gameState == "HordeMode") { hordeMode.SetActive(false); }
    }
    void buttonLeaderboards()
    {
        if (gameState == "Main") { leaderBoards.SetActive(true); }
        if (gameState == "Settings") { leaderBoards.SetActive(false); }
        if (gameState == "Leaderboard") { leaderBoards.SetActive(true); }
        if (gameState == "Mission") { leaderBoards.SetActive(false); }
        if (gameState == "HordeMode") { leaderBoards.SetActive(false); }
    }
    void buttonLeaderboardScreen()
    {
        if (gameState == "Main") { leaderboardScreen.SetActive(false); }
        if (gameState == "Settings") { leaderboardScreen.SetActive(false); }
        if (gameState == "Leaderboard") { leaderboardScreen.SetActive(true); }
        if (gameState == "Mission") { leaderboardScreen.SetActive(false); }
        if (gameState == "HordeMode") { leaderboardScreen.SetActive(false); }
    }
    void buttonSettingsScreen()
    {
        if (gameState == "Main") { //StartCoroutine(Deactivate_settingsScreen());
            settingsScreen.SetActive(false);}
        if (gameState == "Settings") { settingsScreen.SetActive(true); }//settingsScreen.GetComponent<Animator>().SetBool("active", true); } //settingsScreen.GetComponent<Image>().enabled = true; }
        if (gameState == "Leaderboard") {//StartCoroutine(Deactivate_settingsScreen());
            settingsScreen.SetActive(false);
        }
        if (gameState == "Mission") {//StartCoroutine(Deactivate_settingsScreen());
            settingsScreen.SetActive(false);
        }
        if (gameState == "HordeMode") {//StartCoroutine(Deactivate_settingsScreen());
            settingsScreen.SetActive(false);
        }
    }
    public void buttonMissionScreen()
    {
            if (gameState == "Main") { missionScreen.SetActive(false); }
            if (gameState == "Settings") { missionScreen.SetActive(false); }
            if (gameState == "Leaderboard") { missionScreen.SetActive(false); }
            if (gameState == "Mission")
            {
                if (missionState == "main")
                {
                    missionScreen.SetActive(true);
                    missionScreenInfo.SetActive(false);
                    xpScreen.SetActive(false);
                }
                else if (missionState == "mission1" || missionState == "mission2" || missionState == "mission3")
                {
                    missionScreen.SetActive(false);
                    missionScreenInfo.SetActive(true);
                }else if(missionState == "XPScreen"){
                    Debug.Log("mission state is xpscreen");
                    missionScreen.SetActive(false);
                    missionScreenInfo.SetActive(false);
                    xpScreen.SetActive(true);
                }
            }
            if (gameState == "HordeMode") { missionScreen.SetActive(false); }
       
           
    }
    public void onMission1Info()
    {
        missionState = "mission1";
        gameObject.GetComponent<MissionManager>().update_mission_info();
        buttonMissionScreen();
    }
    public void onMission2Info()
    {
        missionState = "mission2";
        gameObject.GetComponent<MissionManager>().update_mission_info();
        buttonMissionScreen();
    }
    public void onMission3Info()
    {
        missionState = "mission3";
        gameObject.GetComponent<MissionManager>().update_mission_info();
        buttonMissionScreen();
    }

    void setTotalRoundText(){
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        master_obj.GetComponent<MasterStats>().refresh_maxRoundSum();
        totalRoundText.GetComponent<Text>().text = "Total Rounds: " +   master_obj.GetComponent<MasterStats>().maxRoundSum;
       
    }
    void refreshLevelUIs()
    {
        int totalMaxRounds = Managers.stats.maxRoundSum;
        level1.GetComponent<Image>().overrideSprite = levelsUnlocked[0];
        level2.GetComponent<Image>().overrideSprite = levelsLocked[1];
        level3.GetComponent<Image>().overrideSprite = levelsLocked[2];
        level4.GetComponent<Image>().overrideSprite = levelsLocked[3];
        level5.GetComponent<Image>().overrideSprite = levelsLocked[4];
        level6.GetComponent<Image>().overrideSprite = levelsLocked[5];

        level1.transform.GetChild(1).gameObject.SetActive(false);
        level1.transform.GetChild(2).gameObject.SetActive(false);
        level1.transform.GetChild(3).gameObject.SetActive(false);
        level1.transform.GetChild(4).gameObject.SetActive(true);

        level2.transform.GetChild(1).gameObject.SetActive(false);
        level2.transform.GetChild(2).gameObject.SetActive(false);
        level2.transform.GetChild(3).gameObject.SetActive(false);
        level2.transform.GetChild(4).gameObject.SetActive(true);

        level3.transform.GetChild(1).gameObject.SetActive(false);
        level3.transform.GetChild(2).gameObject.SetActive(false);
        level3.transform.GetChild(3).gameObject.SetActive(false);
        level3.transform.GetChild(4).gameObject.SetActive(true);

        level4.transform.GetChild(1).gameObject.SetActive(false);
        level4.transform.GetChild(2).gameObject.SetActive(false);
        level4.transform.GetChild(3).gameObject.SetActive(false);
        level4.transform.GetChild(4).gameObject.SetActive(true);

        level5.transform.GetChild(1).gameObject.SetActive(false);
        level5.transform.GetChild(2).gameObject.SetActive(false);
        level5.transform.GetChild(3).gameObject.SetActive(false);
        level5.transform.GetChild(4).gameObject.SetActive(true);

        level6.transform.GetChild(1).gameObject.SetActive(false);
        level6.transform.GetChild(2).gameObject.SetActive(false);
        level6.transform.GetChild(3).gameObject.SetActive(false);
        level6.transform.GetChild(4).gameObject.SetActive(true);

        if (totalMaxRounds >= 0)
        {
            level1.GetComponent<Image>().overrideSprite = levelsUnlocked[0];
            level1.transform.GetChild(1).gameObject.SetActive(true);
            level1.transform.GetChild(2).gameObject.SetActive(true);
            level1.transform.GetChild(3).gameObject.SetActive(true);
            level1.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (totalMaxRounds > 10){ 
            level2.GetComponent<Image>().overrideSprite = levelsUnlocked[1];
            level2.transform.GetChild(1).gameObject.SetActive(true);
            level2.transform.GetChild(2).gameObject.SetActive(true);
            level2.transform.GetChild(3).gameObject.SetActive(true);
            level2.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (totalMaxRounds > 20) { level3.GetComponent<Image>().overrideSprite = levelsUnlocked[2]; 
            level3.transform.GetChild(1).gameObject.SetActive(true);
            level3.transform.GetChild(2).gameObject.SetActive(true);
            level3.transform.GetChild(3).gameObject.SetActive(true);
            level3.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (totalMaxRounds > 30) { level4.GetComponent<Image>().overrideSprite = levelsUnlocked[3]; 
            level4.transform.GetChild(1).gameObject.SetActive(true);
            level4.transform.GetChild(2).gameObject.SetActive(true);
            level4.transform.GetChild(3).gameObject.SetActive(true);
            level4.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (totalMaxRounds > 40) { level5.GetComponent<Image>().overrideSprite = levelsUnlocked[4]; 
            level5.transform.GetChild(1).gameObject.SetActive(true);
            level5.transform.GetChild(2).gameObject.SetActive(true);
            level5.transform.GetChild(3).gameObject.SetActive(true);
            level5.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (totalMaxRounds > 50) { level6.GetComponent<Image>().overrideSprite = levelsUnlocked[5]; 
            level6.transform.GetChild(1).gameObject.SetActive(true);
            level6.transform.GetChild(2).gameObject.SetActive(true);
            level6.transform.GetChild(3).gameObject.SetActive(true);
            level6.transform.GetChild(4).gameObject.SetActive(false);
        }
    }
    void Awake()
    {
        gameState = "Main";
        refreshButtons();
    }
    void Start () {
        refreshButtons();
	}
    void Update () {
        //refreshButtons();
    }
    public void onHordeMode()
    {
        gameState = "HordeMode";
        refreshButtons();
    }
    public void onMission()
    {
        gameState = "Mission";
        missionState = "main";
        refreshButtons();
    }
    public void onSettings()
    {
        if (gameState != "Settings")
        {
            gameState = "Settings";
            refreshButtons();
            return;
        }
        if (gameState == "Settings")
        {
            gameState = "Main";
            refreshButtons();
            return;
        }
    }
    public void onLeaderBoards()
    {
        if (gameState != "Leaderboard")
        {
            gameState = "Leaderboard";
            refreshButtons();
            return;
        }
        if (gameState == "Leaderboard")
        {
            gameState = "Main";
            refreshButtons();
            return;
        }
    }
    public void onMute()
    {
        refreshButtons();
    }
    public void onBack()
    {
        if (gameState != "Mission")
        {
            gameState = "Main";
            refreshButtons();
        }
        else if (missionState == "main")
        {
            gameState = "Main";
            refreshButtons();
        }
        else if (missionState == "mission1" || missionState == "mission2" || missionState == "mission3")
        {
            gameState = "Mission";
            missionState = "main";
            refreshButtons();
        }

    }
    public void onLevel1()
    {
        Managers.player.currentSceneNumber = 1;
        SceneManager.LoadScene("level1"); 
    }
    public void onLevel2()
    {
        Managers.player.currentSceneNumber = 2;
        if (Managers.stats.maxRoundSum >= 10) { SceneManager.LoadScene("level2"); }
    }
    public void onLevel3()
    {
        Managers.player.currentSceneNumber = 3;
        if (Managers.stats.maxRoundSum >= 20) { SceneManager.LoadScene("level3"); }
    }
    public void onLevel4()
    {
        Managers.player.currentSceneNumber = 4;
        if (Managers.stats.maxRoundSum >= 30) { SceneManager.LoadScene("level4"); }
    }
    public void onLevel5()
    {
        Managers.player.currentSceneNumber = 5;
        if (Managers.stats.maxRoundSum >= 40) { SceneManager.LoadScene("level5"); }
    }
    public void onLevel6()
    {
        Managers.player.currentSceneNumber = 6;
        if (Managers.stats.maxRoundSum >= 50) {SceneManager.LoadScene("level6"); }  
    }








    private IEnumerator Deactivate_leaderboardScreen()
    {   leaderboardScreen.GetComponent<Animator>().SetBool("active", false);
        yield return new WaitForSeconds(2);
        leaderboardScreen.SetActive(false);}
    

    private IEnumerator Deactivate_settingsScreen()
    {
        settingsScreen.GetComponent<Animator>().SetBool("active", false);
        yield return new WaitForSeconds(2);
        //settingsScreen.GetComponent<Image>().enabled = false;
    }

    private IEnumerator Deactivate_missionScreen()
    {
        missionScreen.GetComponent<Animator>().SetBool("active", false);
        yield return new WaitForSeconds(2);
        missionScreen.SetActive(false);
    }
    private IEnumerator Deactivate_mute()
    {
        mute.GetComponent<Animator>().SetBool("active", false);
        yield return new WaitForSeconds(2);
        //mute.GetComponent<Image>().enabled = false;
    }
 
    public void mission_info_setup(string progress, string target, string percent, string xp, string difficulty,int mission_id){
        missionScreenInfoObjects[0].GetComponent<Text>().text = progress;
        missionScreenInfoObjects[1].GetComponent<Text>().text = target;
        missionScreenInfoObjects[2].GetComponent<Text>().text = percent;
        missionScreenInfoObjects[3].GetComponent<Text>().text = xp;
        missionScreenInfoObjects[4].GetComponent<Text>().text = difficulty;
        missionScreenInfoObjects[5].GetComponent<Text>().text = "Mission " + mission_id + " Info";
        if(gameObject.GetComponent<MissionManager>().is_mission_complete(mission_id) == true){
            mission_info_button.active = true;
        }else{
            mission_info_button.active = false;
        }
    }
    public void on_complete_mission(){
        int mission_id = int.Parse(missionState.Substring(7));
        Debug.Log("mission_id  " + mission_id);
        gameObject.GetComponent<MissionManager>().complete_mission(mission_id);
        //missionState = "main";
        //refreshButtons();

    }
}
