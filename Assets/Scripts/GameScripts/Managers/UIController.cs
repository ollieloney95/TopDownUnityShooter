using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    [SerializeField] private Text multiplierText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text roundText;
    [SerializeField] private Image slot1UI;
    [SerializeField] private Image slot2UI;
    [SerializeField] private Image slot1Image;
    [SerializeField] private Image slot2Image;
    [SerializeField] private Image bloodSplat;
    [SerializeField] public Sprite[] gunImageList;

    public GameObject helicopter_rope;
    public GameObject helicopter_light;

    public GameObject death_screen_obj;

	[SerializeField] public Camera playerFaceCam;
	[SerializeField] public GameObject playerFaceImage;
	public RenderTexture targetTexture;

    public GameObject helicopter;
    public GameObject heli_cam;
    public GameObject temp_player;
    public GameObject main_cam;
    public GameObject heli_cam_actual;


    [SerializeField] public GameObject x2damageProgress;
    [SerializeField] public GameObject x2scoreProgress;
    [SerializeField] public GameObject x2moneyProgress;
    [SerializeField] public GameObject ammopackProgress;
    [SerializeField] public GameObject medipackProgress;
    [SerializeField] public GameObject unlimitedclipProgress;

    [SerializeField] public GameObject DamageFront;
    [SerializeField] public GameObject FireRateFront;
    [SerializeField] public GameObject ClipSizeFront;
    [SerializeField] public GameObject MaxAmmoFront;
    [SerializeField] public GameObject ReloadSpeedFront;
    [SerializeField] public GameObject WeightFront;

    [SerializeField] public GameObject DamageFront2;
    [SerializeField] public GameObject FireRateFront2;
    [SerializeField] public GameObject ClipSizeFront2;
    [SerializeField] public GameObject MaxAmmoFront2;
    [SerializeField] public GameObject ReloadSpeedFront2;
    [SerializeField] public GameObject WeightFront2;

    [SerializeField] public GameObject GunImage1;
    [SerializeField] public GameObject GunImage2;

    private bool bleeding;

    private bool x2damage;
    private bool x2money;
    private bool x2score;
    private bool unlimitedclip;
    private bool ammopack;
    private bool medipack;

    private double timeSinceMain = 0;

    [SerializeField]    private GameObject mainGameUIS;
    [SerializeField]    private GameObject[] staticMainUis;
    [SerializeField]    private GameObject slot1;
    [SerializeField]    private GameObject slot2;
    [SerializeField]    private GameObject settings;
    [SerializeField]    private GameObject mute;
    [SerializeField]    private GameObject bag;
    [SerializeField]    private GameObject pause;
    [SerializeField]    private GameObject tl_ui;
    [SerializeField]    private GameObject multiplier;
    [SerializeField]    private GameObject inventory;
    [SerializeField]    private GameObject statsBackground;
    [SerializeField]    private GameObject settingsPopup;
    [SerializeField]    private GameObject pausePopup;
    [SerializeField]    private GameObject roundProgressBack;
    [SerializeField]    private GameObject roundProgressFront;
	[SerializeField]    private GameObject healthbarFront;
	[SerializeField]    private GameObject ammoBarFront;
    [SerializeField]    private GameObject outOfAmmo;
    [SerializeField]    private GameObject reloading;
    [SerializeField]    private GameObject deathImages;

    [SerializeField] public GameObject[] perkLevelImages;
    [SerializeField] public Sprite[] perkLevels;
    public string gs;


    [SerializeField] public static string gameState;
    [SerializeField] private ArrayList gameStates;

    
    void setUpgameStates()
    {
        gameStates.Add("Starting");
        gameStates.Add("Main");
        gameStates.Add("Settings");
        gameStates.Add("Bag");
        gameStates.Add("Pause");
        gameStates.Add("Death");
    }
    void refreshMainGameUIS()
    {
        if (gameState == "Starting") { mainGameUIS.SetActive(false); }
        if (gameState == "Main") { mainGameUIS.SetActive(true); }
        if (gameState == "Settings") { mainGameUIS.SetActive(false); }
        if (gameState == "Bag") { mainGameUIS.SetActive(true); }
        if (gameState == "Pause") { mainGameUIS.SetActive(false); }
        if (gameState == "Death") { mainGameUIS.SetActive(false); }
    }
    void refreshButtons()
    {
        buttonSlot1();
        buttonSlot2();
        buttonSettings();
        buttonInventory();
        buttonMute();
        buttonPause();
        buttonMultiplier();
        buttonPausePopup();
        buttonSettingsPopup();
        textAmmo();
        textHealth();
        textScore();
        textMultiplier();
        textMoney();
        buttonstatsBackground();
        buttonBag();
        top_left_ui();
        refreshMainGameUIS();
        buttonDeathImages();
        refreshCamBlur();
        refreshSettingsGuns();
        refreshProgressBar();
    }
    void refreshText()
    {
        textAmmo();
        textHealth();
        textScore();
        textMultiplier();
        textMoney();
    }
    void refreshSettingsGuns()
    {
        /*
        GameObject.Find("DamageFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80* Managers.gun.reloadSpeedList[Managers.gun.slot1] / 10f, 10f);
        GameObject.Find("FireRateFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.fireRateList[Managers.gun.slot1] / 1f, 10f);
        GameObject.Find("ClipSizeFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.clipSizeList[Managers.gun.slot1] / 100f, 10f);
        GameObject.Find("WeightFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.gunWeightList[Managers.gun.slot1] / 1f, 10f);
        GameObject.Find("MaxAmmoFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.maxAmmoList[Managers.gun.slot1] / 600f, 10f);
        GameObject.Find("ReloadSpeedFront").GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.reloadSpeedList[Managers.gun.slot1] / 5f, 10f);
        */
        DamageFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.reloadSpeedList[Managers.gun.slot1] / 10f, 10f);
        FireRateFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.fireRateList[Managers.gun.slot1] / 1f, 10f);
        ClipSizeFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.clipSizeList[Managers.gun.slot1] / 100f, 10f);
        WeightFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.gunWeightList[Managers.gun.slot1] / 1f, 10f);
        MaxAmmoFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.maxAmmoList[Managers.gun.slot1] / 600f, 10f);
        ReloadSpeedFront.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.reloadSpeedList[Managers.gun.slot1] / 5f, 10f);

        DamageFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.reloadSpeedList[Managers.gun.slot2] / 10f, 10f);
        FireRateFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.fireRateList[Managers.gun.slot2] / 1f, 10f);
        ClipSizeFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.clipSizeList[Managers.gun.slot2] / 100f, 10f);
        WeightFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.gunWeightList[Managers.gun.slot2] / 1f, 10f);
        MaxAmmoFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.maxAmmoList[Managers.gun.slot2] / 600f, 10f);
        ReloadSpeedFront2.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Managers.gun.reloadSpeedList[Managers.gun.slot2] / 5f, 10f);

        GunImage1.GetComponent<Image>().overrideSprite = gunImageList[Managers.gun.gunIDList[Managers.gun.slot1]];
        GunImage2.GetComponent<Image>().overrideSprite = gunImageList[Managers.gun.gunIDList[Managers.gun.slot2]];
    }
    public void refreshRoundText(){
        roundText.GetComponent<Text>().text = "round: " + Managers.game.currentRound;
    }
    void textMoney()
    {
        moneyText.text = "$"+Managers.game.currentMoney;
    }
    void textAmmo()
    {
        if (Managers.gun.activeSlot == 1) { ammoText.text = "" + Managers.gun.ammoInClipSlot1 + "/" + (Managers.gun.ammoSlot1- Managers.gun.ammoInClipSlot1); }
        if (Managers.gun.activeSlot == 2) { ammoText.text = "" + Managers.gun.ammoInClipSlot2 + "/" + (Managers.gun.ammoSlot2- Managers.gun.ammoInClipSlot2); }
    }
    void textHealth()
    {
        healthText.text = "" + Managers.player.currentHealth +"/"+ Managers.player.maxHealth;
    }
    void textMultiplier()
    {
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        Managers.game.currentMultiplier = master_obj.GetComponent<MasterStats>().multiplier_level;
        multiplierText.text = "x" + Managers.game.currentMultiplier;
    }
    void textScore()
    {
        scoreText.text = "" + Managers.game.currentScore;
    }
    void refreshCamBlur()
    {
        if (gameState == "Main" || gameState == "Starting") {
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
        }
        else
        {
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = true;
        }
    }
    void buttonSlot1()
    {
        if (gameState == "Main") { slot1.SetActive(true); slot1Image.gameObject.SetActive(true);}
        if (gameState == "Settings") { slot1.SetActive(false); slot1Image.gameObject.SetActive(false); }
        if (gameState == "Bag") { slot1.SetActive(false); slot1Image.gameObject.SetActive(false);}
        if (gameState == "Pause") { slot1.SetActive(false); slot1Image.gameObject.SetActive(false);}
        if (gameState == "Death") { slot1.SetActive(false); slot1Image.gameObject.SetActive(false);}
        if (gameState == "Starting") { slot1.SetActive(false); slot1Image.gameObject.SetActive(false); }
    }
    public void splat()
    {
        StartCoroutine(bloodSplatCo());
    }
    IEnumerator bloodSplatCo()
    {
        bloodSplat.enabled = true;
        bloodSplat.GetComponent<Image>().color = Color.white;
        bleeding = true;
        yield return new WaitForSeconds(1f);
        bleeding = false;
    }
    void buttonSlot2()
    {
        if (gameState == "Main") { slot2.SetActive(true); slot2Image.gameObject.SetActive(true);}
        if (gameState == "Settings") { slot2.SetActive(false); slot2Image.gameObject.SetActive(false);}
        if (gameState == "Bag") { slot2.SetActive(false); slot2Image.gameObject.SetActive(false);}
        if (gameState == "Pause") { slot2.SetActive(false); slot2Image.gameObject.SetActive(false); }
        if (gameState == "Death") { slot2.SetActive(false); slot2Image.gameObject.SetActive(false);}
        if (gameState == "Starting") { slot2.SetActive(false); slot2Image.gameObject.SetActive(false); }
    }
    void buttonDeathImages()
    {
        if (gameState == "Main") { deathImages.SetActive(false); }
        if (gameState == "Settings") { deathImages.SetActive(false); }
        if (gameState == "Bag") { deathImages.SetActive(false); }
        if (gameState == "Pause") { deathImages.SetActive(false); }
        if (gameState == "Death") { deathImages.SetActive(true); }
        if (gameState == "Starting") { deathImages.SetActive(false); }
    }
    void buttonSettings()
    {
        if (gameState == "Main") { settings.SetActive(true); }
        if (gameState == "Settings") { settings.SetActive(false); }
        if (gameState == "Bag") { settings.SetActive(false); }
        if (gameState == "Pause") { settings.SetActive(false); }
        if (gameState == "Death") { settings.SetActive(false); }
        if(gameState == "Starting") { settings.SetActive(false); }
    }
    void buttonMute()
    {
        if (gameState == "Main") { mute.SetActive(true); }
        if (gameState == "Settings") { mute.SetActive(false); }
        if (gameState == "Bag") { mute.SetActive(false); }
        if (gameState == "Pause") { mute.SetActive(false); }
        if (gameState == "Death") { mute.SetActive(false); }
        if (gameState == "Starting") { mute.SetActive(false); }
    }
    void buttonBag()
    {
        if (gameState == "Main") { bag.SetActive(true); }
        if (gameState == "Settings") { bag.SetActive(false); }
        if (gameState == "Bag") { bag.SetActive(false); }
        if (gameState == "Pause") { bag.SetActive(false); }
        if (gameState == "Death") { bag.SetActive(false); }
        if (gameState == "Starting") { bag.SetActive(false); }
    }
    void top_left_ui(){
        if (gameState == "Main") { tl_ui.SetActive(true); }
        if (gameState == "Settings") { tl_ui.SetActive(false); }
        if (gameState == "Bag") { tl_ui.SetActive(false); }
        if (gameState == "Pause") { tl_ui.SetActive(false); }
        if (gameState == "Death") { tl_ui.SetActive(false); }
        if (gameState == "Starting") { tl_ui.SetActive(false); }
    }
    void buttonPause()
    {
        if (gameState == "Main") { pause.SetActive(true); }
        if (gameState == "Settings") { pause.SetActive(false); }
        if (gameState == "Bag") { pause.SetActive(false); }
        if (gameState == "Pause") { pause.SetActive(false); }
        if (gameState == "Death") { pause.SetActive(false); }
        if (gameState == "Starting") { pause.SetActive(false); }
    }
    void buttonInventory()
    {
        if (gameState == "Main") { inventory.SetActive(false); }
        if (gameState == "Settings") { inventory.SetActive(false); }
        if (gameState == "Bag") { inventory.SetActive(true); }
        if (gameState == "Pause") { inventory.SetActive(false); }
        if (gameState == "Death") { inventory.SetActive(false); }
        if (gameState == "Starting") { inventory.SetActive(false); }
    }
    void buttonstatsBackground()
    {
        if (gameState == "Main") { statsBackground.SetActive(true); }
        if (gameState == "Settings") { statsBackground.SetActive(false); }
        if (gameState == "Bag") { statsBackground.SetActive(false); }
        if (gameState == "Pause") { statsBackground.SetActive(false); }
        if (gameState == "Death") { statsBackground.SetActive(false); }
        if (gameState == "Starting") { statsBackground.SetActive(false); }
    }
    void buttonMultiplier()
    {
        if (gameState == "Main") { multiplier.SetActive(true); }
        if (gameState == "Settings") { multiplier.SetActive(false); }
        if (gameState == "Bag") { multiplier.SetActive(false); }
        if (gameState == "Pause") { multiplier.SetActive(false); }
        if (gameState == "Death") { multiplier.SetActive(false); }
        if (gameState == "Starting") { multiplier.SetActive(false); }
    }
    void buttonSettingsPopup()
    {
        if (gameState == "Main") { settingsPopup.SetActive(false); }
        if (gameState == "Settings") { settingsPopup.SetActive(true); }
        if (gameState == "Bag") { settingsPopup.SetActive(false); }
        if (gameState == "Pause") { settingsPopup.SetActive(false); }
        if (gameState == "Death") { settingsPopup.SetActive(false); }
        if (gameState == "Starting") { settingsPopup.SetActive(false); }
    }
    void buttonPausePopup()
    {
        if (gameState == "Main") { pausePopup.SetActive(false); }
        if (gameState == "Settings") { pausePopup.SetActive(false); }
        if (gameState == "Bag") { pausePopup.SetActive(false); }
        if (gameState == "Pause") { pausePopup.SetActive(true); }
        if (gameState == "Death") { pausePopup.SetActive(false); }
        if (gameState == "Starting") { pausePopup.SetActive(false); }
    }

    //to set

    //static in play UI's
    void showStaticMainUIs(bool show){
        if(show==true){
            foreach(GameObject go in staticMainUis){
                go.SetActive(true);
            }
        }else{
            foreach(GameObject go in staticMainUis){
                go.SetActive(false);
            }
        }
    }
    void staticMainUIs()
    {
        if (gameState == "Main") { showStaticMainUIs(true); }
        if (gameState == "Settings") { showStaticMainUIs(false); }
        if (gameState == "Bag") { showStaticMainUIs(false); }
        if (gameState == "Pause") { showStaticMainUIs(false); }
        if (gameState == "Death") { showStaticMainUIs(false);}
        if (gameState == "Starting") { showStaticMainUIs(false); }
    }


    void Awake()
    {
        gameState = "Starting";
		slot1UI.sprite = Resources.Load<Sprite>("UI/120x120slotselectedTransparent");
        Messenger.AddListener(GameEvent.ammopack, onAmmopackX);
        Messenger.AddListener(GameEvent.x2damage, onx2damageX);
        Messenger.AddListener(GameEvent.unlimitedclip, onUnlimitedclipX);
        Messenger.AddListener(GameEvent.medipack, onMedipackX);
        Messenger.AddListener(GameEvent.x2money, onx2moneyX);
        Messenger.AddListener(GameEvent.x2score, onx2scoreX);
        //gameStates.Add("Main");
        //gameStates.Add("Settings");
        //gameStates.Add("Bag");
        //gameStates.Add("Pause"

    }
    void Start () {
        gameState = "Starting";
        StartCoroutine(Starting_level());
        refreshButtons();
        refreshText();
    }
    IEnumerator Starting_level()
    {
        helicopter_rope.GetComponent<MeshRenderer>().enabled = false;
        helicopter_light.GetComponent<Light>().enabled = false;
        GameObject player_obj;
        player_obj = GameObject.FindGameObjectsWithTag("Player")[0];
        player_obj.active = false;
        Vector3 helicopter_init_pos;
        Vector3 helicopter_final_pos;
        helicopter_init_pos = helicopter.transform.position;
        helicopter_final_pos = new Vector3(-17,10,0);
        float startTime = Time.time;
        StartCoroutine(camera360());
        while (Time.time < startTime + 10f)
        {
            //helicopter.transform.position = Vector3.Lerp(helicopter_init_pos.position, helicopter_final_pos, (Time.time - startTime) / 500f);
            helicopter.transform.position = myLerp(helicopter_init_pos, helicopter_final_pos, (Time.time - startTime) / 10f);
            //yield return(null);
            yield return new WaitForEndOfFrame();
        }
        helicopter.transform.position = helicopter_final_pos;
        startTime = Time.time;
        Vector3 temp_player_init_pos;
        temp_player_init_pos = temp_player.transform.position;
        Vector3 temp_player_final_pos;
        temp_player_final_pos = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        Vector3 heli_cam_init_pos;
        heli_cam_init_pos = heli_cam_actual.transform.position;
        helicopter_rope.GetComponent<MeshRenderer>().enabled = true;
        helicopter_light.GetComponent<Light>().enabled = true;
        while (Time.time < startTime + 4f)
        {
            temp_player.active = true;
            //temp_player.transform.position = Vector3.Lerp(temp_player_init_pos.position, temp_player_final_pos.position, (Time.time - startTime) / 200f);
            //heli_cam_actual.transform.position = Vector3.Lerp(heli_cam_init_pos.position, heli_cam_init_pos.position - new Vector3(0,2,0), (Time.time - startTime) / 200f);
            temp_player.transform.position = myLerp(temp_player_init_pos, temp_player_final_pos, (Time.time - startTime) / 4f);
            heli_cam_actual.transform.position = myLerp(heli_cam_init_pos, heli_cam_init_pos - new Vector3(0, 5, 0), (Time.time - startTime) / 4f);
            yield return new WaitForEndOfFrame();
        }
        helicopter_light.GetComponent<Light>().enabled = false;
        helicopter_rope.GetComponent<MeshRenderer>().enabled = false;
        player_obj.active = true;
        temp_player.transform.position = temp_player_final_pos;
        Destroy(temp_player);
        StartCoroutine(cameraToMain());
    }
    IEnumerator camera360()
    {
        float startTime = Time.time;
        while (Time.time < startTime + 10f)
        {
            heli_cam.transform.localEulerAngles = new Vector3(0, 0, 36f * (Time.time - startTime));
            //helicopter.transform.position = Vector3.Lerp(helicopter_init_pos.position, helicopter_final_pos, (Time.time - startTime) / 100f);
            //yield return (null);
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator cameraToMain()
    {
        float startTime = Time.time;
        Vector3 heli_cam_init_pos;
        Quaternion heli_cam_init_rot;
        heli_cam_init_pos = heli_cam_actual.transform.position;
        heli_cam_init_rot = heli_cam_actual.transform.rotation;
        while (Time.time < startTime + 3f)
        {
            heli_cam_actual.transform.rotation = Quaternion.Lerp(heli_cam_init_rot, main_cam.transform.rotation, (Time.time - startTime) / 3f);
            //heli_cam_actual.transform.position = Vector3.Lerp(heli_cam_init_pos.position, main_cam.transform.position, (Time.time - startTime) / 100f);
            heli_cam_actual.transform.position = myLerp(heli_cam_init_pos, main_cam.transform.position, (Time.time - startTime) / 3f);
            //helicopter.transform.position = Vector3.Lerp(helicopter_init_pos.position, helicopter_final_pos, (Time.time - startTime) / 100f);
            yield return new WaitForEndOfFrame();
        }
        gameState = "Main";
        GameObject.FindGameObjectsWithTag("Player")[0].active = true;
        refreshButtons();
        Destroy(heli_cam_actual);
        Destroy(helicopter);
    }
    Vector3 myLerp(Vector3 v1, Vector3 v2, float frac){
        Vector3 newVec = new Vector3();
        newVec.x = (v2.x - v1.x) * frac  + v1.x;
        newVec.y = (v2.y - v1.y) * frac + v1.y;
        newVec.z = (v2.z - v1.z) * frac + v1.z;
        return (newVec);
    }
    void refreshProgressBar()
    {
		healthbarFront.GetComponent<RectTransform> ().sizeDelta = new Vector2 (198f * Managers.player.currentHealth / Managers.player.maxHealth, 8f);
		if(Managers.gun.activeSlot==1){
			ammoBarFront.GetComponent<RectTransform> ().sizeDelta = new Vector2 (198f * Managers.gun.ammoInClipSlot1 / Managers.gun.clipSize,8f);
		} else {
			ammoBarFront.GetComponent<RectTransform> ().sizeDelta = new Vector2 (198f * Managers.gun.ammoInClipSlot2 / Managers.gun.clipSize, 8f);
		}
		if (Managers.game.loadingRound == false) {
			roundProgressFront.GetComponent<RectTransform> ().sizeDelta = new Vector2 (10, 350f * ((float)Managers.game.enemiesInRound - (float)Managers.game.enemiesAlive) / ((float)Managers.game.enemiesInRound));
		}else{
            //make progress bar go back to zero for the start of the next round
            float _rectY = roundProgressFront.GetComponent<RectTransform>().sizeDelta.y;
            roundProgressFront.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 350f*(Time.deltaTime / Managers.game.timeBetweenRounds));
        }
        if (gameState == "Starting") { roundProgressBack.SetActive(false); roundProgressFront.SetActive(false); }
        if (gameState == "Main") { roundProgressBack.SetActive(true); roundProgressFront.SetActive(true); }
        if (gameState == "Settings") { roundProgressBack.SetActive(false); roundProgressFront.SetActive(false); }
        if (gameState == "Bag") { roundProgressBack.SetActive(false); roundProgressFront.SetActive(false); }
        if (gameState == "Pause") { roundProgressBack.SetActive(false); roundProgressFront.SetActive(false); }
        if (gameState == "Death") { roundProgressBack.SetActive(false); roundProgressFront.SetActive(false); }
    }
    void Update () {//////////////////////////////////////////////////////////////////////
        bloodSplat.GetComponent<Image>().color = Color.Lerp(bloodSplat.GetComponent<Image>().color, Color.clear, 2 * Time.deltaTime / 1f);
        if(gameState != "Main"){
            bloodSplat.GetComponent<Image>().color = Color.clear;
        }
        gs = gameState;
        //if (Input.GetMouseButton(0)   && !EventSystem.current.IsPointerOverGameObject() && gameState!="Death")
        //{
        //    gameState = "Main";
        //    refreshButtons();
        //}

        if (timeSinceMain > 0){
            if (gameState == "Bag" && Input.GetMouseButtonUp(0) &&( Mathf.Abs(Input.mousePosition.x - Screen.width / 2f) > Screen.width * (210f / 856f) || Mathf.Abs(Input.mousePosition.y - Screen.height / 2f) > Screen.height * (158f / 600f) ))
            {
                gameState = "Main";
                refreshButtons();
            }
            if (gameState == "Settings" && Input.GetMouseButtonUp(0) &&( Mathf.Abs(Input.mousePosition.x - Screen.width / 2f) > Screen.width * (300f / 856f) || Mathf.Abs(Input.mousePosition.y - Screen.height / 2f) > Screen.height * (150f / 600f) ))
            {
                gameState = "Main";
                refreshButtons();
            }
            if (gameState == "Pause" && Input.GetMouseButtonUp(0) &&( Mathf.Abs(Input.mousePosition.x - Screen.width / 2f) > Screen.width * (150f / 856f) || Mathf.Abs(Input.mousePosition.y - Screen.height / 2f) > Screen.height * (50f / 600f) ))
            {
                gameState = "Main";
                refreshButtons();
            }
        }
        if (gameState != "Main")
        {
            timeSinceMain += Time.deltaTime;
            if(gameState == "Death"){
                refreshPause();
            }
        }
        else
        {
            timeSinceMain = 0;
        }
        refreshText();
        refreshReloadAndAmmo();
        refreshPerkLevel();
        refreshPause();
        refreshx2damageProgress();
        refreshx2moneyProgress();
        refreshx2scoreProgress();
        refreshunlimitedclipProgress();
        refreshammopackProgress();
        refreshmedipackProgress();
        staticMainUIs();
        refreshProgressBar();
		//if (gameState == "Settings") {
		//	playerFaceCam.enabled = true;
		//} else {
		//	playerFaceCam.enabled = false;
		//}
    }
    void refreshPause()
    {
        if (gameState == "Main") { Managers.game.UnPause();}
        if (gameState == "Settings") { Managers.game.Pause(); }
        if (gameState == "Bag") { Managers.game.Pause(); }
        if (gameState == "Pause") { Managers.game.Pause(); }
        if (gameState == "Death") { Managers.game.Pause(); }
    }
    void refreshReloadAndAmmo()
    {
        if (Managers.gun.reloading == true) { reloading.SetActive(true); } else { reloading.SetActive(false); }
        if ((Managers.gun.activeSlot == 1 && Managers.gun.ammoSlot1 == 0) || (Managers.gun.activeSlot == 2 && Managers.gun.ammoSlot2 == 0)) { outOfAmmo.SetActive(true); reloading.SetActive(false); } else { outOfAmmo.SetActive(false); }
    }
    public void onSlot1()
    {
        if (Managers.gun.activeSlot == 2)
        {
            Managers.gun.toggleSlot();
			slot1UI.sprite = Resources.Load<Sprite>("UI/120x120slotselectedTransparent");
			slot2UI.sprite = Resources.Load<Sprite>("UI/120x120slotunselectedTransparent");
        }
        refreshButtons();
    }
    public void onMainMenu()
    {
        //Application.LoadLevel("");
        SceneManager.LoadScene("MainMenu");
    }
    public void onSlot2()
    {
        if (Managers.gun.activeSlot == 1)
        {
            Managers.gun.toggleSlot();
            slot2UI.sprite = Resources.Load<Sprite>("UI/120x120slotselectedTransparent");
			slot1UI.sprite = Resources.Load<Sprite>("UI/120x120slotunselectedTransparent");
        }
        refreshButtons();
    }
    public void onSettings()
    {
        if (gameState == "Main" || gameState == "Bag" || gameState == "Pause")
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
    public void onMute()
    {

        refreshButtons();
    }
    public void onBag()
    {
        if (gameState == "Main") {
            gameState = "Bag";
            refreshButtons();
            return;
        }
        if (gameState == "Bag")
        {
            gameState = "Main";
            refreshButtons();
            return;
        }
    }
    public void onDeath()
    {
        death_screen_obj.active = true;

        if (gameState != "Death")
        {
            gameState = "Death";
            int this_level = 0;
            int this_round = Managers.game.currentRound;
            int this_score = Managers.game.currentScore;
            int this_kills = Managers.game.currentKills;
            int this_money = Managers.game.currentMoney;
            Managers.stats.update_stats_from_round(this_level, this_round, this_score, this_kills, this_money);
            death_screen_obj.GetComponent<SetDeathScreenInfo>().refresh_death_info();
            refreshButtons();
        }
        refreshButtons();
        gameState = "Death";
    }
    public void onMultiplier()
    {
        //gameState = "";
        refreshButtons();
    }
    public void onPause()
    {
        if (gameState == "Main" || gameState == "Bag")
        {
            gameState = "Pause";
            refreshButtons();
            return;
        }
        if (gameState == "Pause")
        {
            gameState = "Main";
            refreshButtons();
            return;
        }
    }

    public void onBuy()
    {
        Messenger.Broadcast(GameEvent.ONBUY);
    }
    void refreshPerkLevel()
    {
        int[] perkLevelsList = new int[7];
        perkLevelsList[0] = Managers.player.clipSizePerkLevel;
        perkLevelsList[1] = Managers.player.fireRatePerkLevel;
        perkLevelsList[2] = Managers.player.healthPerkLevel;
        perkLevelsList[3] = Managers.player.maxAmmoPerkLevel;
        perkLevelsList[4] = Managers.player.powerPerkLevel;
        perkLevelsList[5] = Managers.player.reloadSpeedPerkLevel;
        perkLevelsList[6] = Managers.player.speedPerkLevel;
        
        for (int i=0; i<7; i++)
        {
            perkLevelImages[i].GetComponent<Image>().overrideSprite = perkLevels[perkLevelsList[i]];
            
           
        }
        refreshGunImages();
    } 
    public void refreshGunImages()
    {
        slot1Image.overrideSprite = gunImageList[Managers.gun.gunIDList[Managers.gun.slot1]];
        slot2Image.overrideSprite = gunImageList[Managers.gun.gunIDList[Managers.gun.slot2]];
    }
    void refreshx2damageProgress()
    {
        float _rectY = x2damageProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        x2damageProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onx2damage()
    {
        x2damageProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        x2damage = true;
        x2damageProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        x2damageProgress.SetActive(false);
        x2damage = false;
        x2damageProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10,50f);
    }
    void refreshx2scoreProgress()
    {
        float _rectY = x2scoreProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        x2scoreProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onx2score()
    {
        x2scoreProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        x2score = true;
        x2scoreProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        x2scoreProgress.SetActive(false);
        x2score = false;
        x2scoreProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
    }
    void refreshx2moneyProgress()
    {
        float _rectY = x2moneyProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        x2moneyProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onx2money()
    {
        x2moneyProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        x2money = true;
        x2moneyProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        x2moneyProgress.SetActive(false);
        x2money = false;
        x2moneyProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
    }
    void refreshmedipackProgress()
    {
        float _rectY = medipackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        medipackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onMedipack()
    {
        medipackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        medipack = true;
        medipackProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        medipackProgress.SetActive(false);
        medipack = false;
        medipackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
    }
    void refreshammopackProgress()
    {
        float _rectY = ammopackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        ammopackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onAmmopack()
    {
        ammopackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        ammopack = true;
        ammopackProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        ammopackProgress.SetActive(false);
        ammopack = false;
        ammopackProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
    }
    void refreshunlimitedclipProgress()
    {
        float _rectY = unlimitedclipProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        unlimitedclipProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, _rectY - 50f * (Time.deltaTime / 20f));
    }
    public IEnumerator onUnlimitedclip()
    {
        unlimitedclipProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
        unlimitedclip = true;
        unlimitedclipProgress.SetActive(true);
        yield return new WaitForSeconds(20);
        unlimitedclipProgress.SetActive(false);
        unlimitedclip = false;
        unlimitedclipProgress.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 50f);
    }
    void onAmmopackX() { StartCoroutine(onAmmopack()); }
    void onMedipackX() { StartCoroutine(onMedipack()); }
    void onx2damageX() { StartCoroutine(onx2damage()); }
    void onx2scoreX() { StartCoroutine(onx2score()); }
    void onx2moneyX() { StartCoroutine(onx2money()); }
    void onUnlimitedclipX() { StartCoroutine(onUnlimitedclip()); }
}
