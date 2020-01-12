using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class GunManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    [SerializeField] public GameObject bullet;
    [SerializeField] public GameObject playersGun;
    [SerializeField] public GameObject bulletSpawnPos;
    [SerializeField] public Transform playerGraphics;
    [SerializeField] public Collider Terrain;
    [SerializeField] public Camera cam;
    [SerializeField] public int activeSlot = 1;
    [SerializeField] public string slot1 = "Pistol";
    [SerializeField] public string slot2 = "unequiped";
    [SerializeField] public GameObject[] gunModels;
    public bool unlimitedClip = false;

    //firing mechanism
    [SerializeField] public bool ReadyToShoot = true;
    [SerializeField] public bool reloading =true;
    private float damageModifier = 1;

    [SerializeField] public string gunEquipped;
    [SerializeField] public int ammoSlot1 = 50;
    [SerializeField] public int ammoSlot2 = 0;
    [SerializeField] public int maxAmmo;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public int ammoInClipSlot1 = 0;
    [SerializeField] public int ammoInClipSlot2 = 0;
    [SerializeField] public int clipSize;
    [SerializeField] public float damage;
    [SerializeField] public float speed;
    [SerializeField] public float weight;
    [SerializeField] public float fireRate;
    [SerializeField] GameObject obj;
    public Transform CameraTrans;
    private float time_since_reload;


    //gun stats 
    void Awake()
    {
        Messenger.AddListener(GameEvent.ammopack, onAmmopack);
        Messenger.AddListener(GameEvent.x2damage, x2damage);
        Messenger.AddListener(GameEvent.unlimitedclip, onUnlimitedclip);
    }
    void x2damage()
    {
        StartCoroutine(onDamageModifier());
    }
    void onUnlimitedclip()
    {
        StartCoroutine(unlimitedclipenum());
    }
    IEnumerator unlimitedclipenum()
    {
        unlimitedClip = true;
        yield return new WaitForSeconds(20f);
        unlimitedClip = false;
    }
    IEnumerator onDamageModifier()
    {
        damageModifier = 2;
        refreshStats();
        yield return new WaitForSeconds(20f);
        damageModifier = 1;
        refreshStats();
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ammopack, onAmmopack);
        Messenger.RemoveListener(GameEvent.x2damage, x2damage);
    }
    void onAmmopack()
    {
        if (activeSlot == 1) {ammoSlot1 = maxAmmo; }
        if (activeSlot == 2) { ammoSlot2 = maxAmmo; }
    }
    public ArrayList gunStats = new ArrayList();
    [SerializeField] public Dictionary<string, int> gunIDList;
    public Dictionary<string, int> maxAmmoList;
    public Dictionary<string, int> clipSizeList;
    public Dictionary<string, float> reloadSpeedList;
    public Dictionary<string, float> fireRateList;
    public Dictionary<string, float> gunWeightList;
    public Dictionary<string, float> gunDamageList;

    

    private void gunStatSetup()
    {
        gunIDList = new Dictionary<string, int>();
        maxAmmoList = new Dictionary<string, int>();
        clipSizeList = new Dictionary<string, int>();
        reloadSpeedList = new Dictionary<string, float>();
        fireRateList = new Dictionary<string, float>();
        gunWeightList = new Dictionary<string, float>();
        gunDamageList = new Dictionary<string, float>();

        //ID's 
        gunIDList["Pistol"] = 0;
        gunIDList["SMG"] = 1;
        gunIDList["LMG"] = 2;
        gunIDList["Rifle"] = 3;
        gunIDList["Sniper"] = 4;
        gunIDList["Shotgun"] = 5;
        gunIDList["unequiped"] = 6;

        //ammo 
        maxAmmoList.Add("Pistol", 120);
        maxAmmoList.Add("SMG", 200);
        maxAmmoList.Add("LMG", 600);
        maxAmmoList.Add("Rifle", 150);
        maxAmmoList.Add("Sniper", 40);
        maxAmmoList.Add("Shotgun", 120);
        maxAmmoList.Add("unequiped", 0);

        //clip size 
        clipSizeList.Add("Pistol", 8);
        clipSizeList.Add("SMG", 30);
        clipSizeList.Add("LMG", 100);
        clipSizeList.Add("Rifle", 16);
        clipSizeList.Add("Sniper", 6);
        clipSizeList.Add("Shotgun", 6);
        clipSizeList.Add("unequiped", 0);

        //reload speed
        reloadSpeedList.Add("Pistol", 1.5f);
        reloadSpeedList.Add("SMG", 1.5f);
        reloadSpeedList.Add("LMG", 5.0f);
        reloadSpeedList.Add("Rifle", 2.0f);
        reloadSpeedList.Add("Sniper", 5.0f);
        reloadSpeedList.Add("Shotgun", 2.0f);
        reloadSpeedList.Add("unequiped", 0f);

        //fire rate
        fireRateList.Add("Pistol", 0.35f);
        fireRateList.Add("SMG", 0.1f);
        fireRateList.Add("LMG", 0.18f);
        fireRateList.Add("Rifle", 0.15f);
        fireRateList.Add("Sniper", 0.5f);
        fireRateList.Add("Shotgun", 0.1f);
        fireRateList.Add("unequiped", 1000f);

        //gun weight
        gunWeightList.Add("Pistol", 0.9f);
        gunWeightList.Add("SMG", 0.8f);
        gunWeightList.Add("LMG", 0.5f);
        gunWeightList.Add("Rifle", 0.8f);
        gunWeightList.Add("Sniper", 0.6f);
        gunWeightList.Add("Shotgun", 0.8f);
        gunWeightList.Add("unequiped", 1f);

        //gun damage 
        gunDamageList.Add("Pistol", 1f);
        gunDamageList.Add("SMG", 1f);
        gunDamageList.Add("LMG", 3f);
        gunDamageList.Add("Rifle", 2f);
        gunDamageList.Add("Sniper", 10f);
        gunDamageList.Add("Shotgun", 10f);
        gunDamageList.Add("unequiped", 0f);

        gunStats.Add(gunIDList);
        gunStats.Add(maxAmmoList);
        gunStats.Add(clipSizeList);
        gunStats.Add(reloadSpeedList);
        gunStats.Add(fireRateList);
        gunStats.Add(gunWeightList);
        gunStats.Add(gunDamageList);

} 
    public void toggleSlot()
    {
        if (activeSlot == 1)
        {
            activeSlot = 2;
            refreshGun(); 
            return;
        }
        activeSlot = 1;
        refreshGun();
        return;
    }
    public void equipGun(string weapon)
    {
        if (activeSlot == 1)
        {
            slot1 = weapon;
        }
        else
        {
            slot2 = weapon;
        }
        refreshGun();
    }
    public void refreshGun()
    {
        for (int i = playersGun.transform.childCount - 1; i >= 0; --i)
        {
            var child = playersGun.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        if (activeSlot == 1)
        {
            gunEquipped = slot1;
            GameObject _newgun = (GameObject)Instantiate(gunModels[gunIDList[slot1]], playersGun.transform.position, playersGun.transform.rotation);
            _newgun.transform.parent = playersGun.transform;
        }
        else
        {
            gunEquipped = slot2;
            GameObject _newgun = (GameObject)Instantiate(gunModels[gunIDList[slot2]], playersGun.transform.position, playersGun.transform.rotation);
            _newgun.transform.parent = playersGun.transform;
        }
        refreshStats();
        //// refreshGunModel();
    }
    public void refreshStats()
    {
        maxAmmo = (int)(maxAmmoList[gunEquipped] * (1 + Managers.player.maxAmmoPerkLevel / 2f));
        clipSize = (int)(clipSizeList[gunEquipped] * (1 + Managers.player.clipSizePerkLevel / 2f));
        reloadSpeed = reloadSpeedList[gunEquipped] * (1 - Managers.player.reloadSpeedPerkLevel / 6f);
        damage = gunDamageList[gunEquipped] * (1+Managers.player.powerPerkLevel) * damageModifier;
        weight = gunWeightList[gunEquipped];
        fireRate = fireRateList[gunEquipped] / (1 + 0.5f *Managers.player.fireRatePerkLevel);
        speed = 1 + (Managers.player.speedPerkLevel/7.0f) * (weight);
    }

	// Use this for initialization
	public void Startup () {
        Debug.Log("Player manager starting...");
        gunStatSetup();
        refreshGun();
        ReadyToShoot = true;
        status = ManagerStatus.Started;
	}
    public IEnumerator onShoot()
    {

        //instantiate bullet here
            Instantiate(bullet, bulletSpawnPos.transform.position, playerGraphics.transform.rotation);
            ReadyToShoot = false;
        if (unlimitedClip == false)
        {
            if (activeSlot == 1)
            {
                ammoInClipSlot1 -= 1;
                ammoSlot1 -= 1;
            }
            else
            {
                ammoInClipSlot2 -= 1;
                ammoSlot2 -= 1;
            }
            time_since_reload = 0f;
            StartCoroutine(muzzle_flash());
        }
        yield return new WaitForSeconds(fireRate);
        if ((activeSlot == 1 && ammoInClipSlot1 > 0) || (activeSlot == 2 && ammoInClipSlot2 > 0))
        {
            ReadyToShoot = true;
        }else
        {
            StartCoroutine(reload());
        }
    }
    public IEnumerator muzzle_flash()
    {
        playersGun.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.active = true;
        yield return new WaitForSeconds(0.1f);
        playersGun.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.active = false;
    }
    public IEnumerator reload()
    {
        reloading = true;
        ReadyToShoot = false;
        yield return new WaitForSeconds(reloadSpeed);
        if(activeSlot == 1)
        {
            ammoInClipSlot1 = clipSize;
            if (ammoInClipSlot1 > ammoSlot1) { ammoInClipSlot1 = ammoSlot1; }
            if (ammoInClipSlot1 == 0) { reloading = false; yield break; }
        }
        else
        {
            ammoInClipSlot2 = clipSize;
            if (ammoInClipSlot2 > ammoSlot2) { ammoInClipSlot2 = ammoSlot2; }
            if (ammoInClipSlot2 == 0) { reloading = false; yield break; }
        }
        reloading = false;
        ReadyToShoot = true;
    }
    public void rotateToMouse()
    {
        if (Application.isEditor == true)   
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            if (Terrain.GetComponent<MeshCollider>().Raycast(ray, out mouseHit, 100f))
            {
                Debug.Log("hit ter");
                Vector3 lookPosition = mouseHit.point;
                lookPosition.y = playerGraphics.transform.position.y;
                playerGraphics.LookAt(lookPosition);
            }
            int layer_mask = LayerMask.GetMask("Aiming");
            Debug.Log("layer_mask  "+layer_mask);
            if (Physics.Raycast(ray, out mouseHit, Mathf.Infinity, layer_mask))
            {
                Debug.Log("hit");
                //Vector3 lookPosition = ray.GetPoint(Camera.main.transform.position.y);
                Vector3 lookPosition = mouseHit.point;
                lookPosition.y = playerGraphics.transform.position.y;
                playerGraphics.LookAt(lookPosition);
                obj.transform.position = lookPosition;
            }
        }
        else
        {
            float yRotation = 0f;
            float horizontal_distance_joystick = Managers.input.GetInput("horizontal_shoot");
            float vertical_distance_joystick = Managers.input.GetInput("vertical_shoot");
            float shoot_joystick_tolerance = 3f;
            Debug.Log("Shoot joystick magnitude:  "+(Mathf.Pow(horizontal_distance_joystick,2) +  Mathf.Pow(vertical_distance_joystick,2)));
            if (horizontal_distance_joystick != 0f )
            {
                yRotation = 180 * Mathf.Atan2(horizontal_distance_joystick, vertical_distance_joystick) / Mathf.PI + CameraTrans.localEulerAngles.y;
                playerGraphics.transform.eulerAngles = new Vector3(playerGraphics.transform.localEulerAngles.x, yRotation, playerGraphics.transform.localEulerAngles.z);
            }
        }
    }
    public void checkReload()
    {
        if (activeSlot == 1 && ammoInClipSlot1 == 0)
        {
            StartCoroutine(reload());
        }
        if (activeSlot == 2 && ammoInClipSlot2 == 0)
        {
            StartCoroutine(reload());
        }
    }
    void Update () {
        rotateToMouse();
        time_since_reload += Time.deltaTime;

        if(time_since_reload > reloadSpeed && reloading == false){
            if(activeSlot == 1)
            {
                ammoInClipSlot1 = clipSize;
                if (ammoInClipSlot1 > ammoSlot1) { ammoInClipSlot1 = ammoSlot1; }
            }
            else
            {
                ammoInClipSlot2 = clipSize;
                if (ammoInClipSlot2 > ammoSlot2) { ammoInClipSlot2 = ammoSlot2; }
            }
            time_since_reload = 0f;
            ReadyToShoot = true;
        }

        float right_stick_distance = 0f;
        right_stick_distance = Mathf.Pow(Mathf.Pow(Managers.input.GetInput("horizontal_shoot"), 2f) + Mathf.Pow(Managers.input.GetInput("vertical_shoot"), 2f), 0.5f);
        //Debug.Log("right_stick_distance" + right_stick_distance);

        if (Application.isEditor == false)
        {
            if (ReadyToShoot == true && right_stick_distance > 0.9f && !EventSystem.current.IsPointerOverGameObject())
            {
                if ((activeSlot == 1 && ammoInClipSlot1 > 0) || (activeSlot == 2 && ammoInClipSlot2 > 0))
                {
                    StartCoroutine(onShoot());
                    ReadyToShoot = false;
                }

            }
        }else{
            if (ReadyToShoot == true && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(0))
            {
                if ((activeSlot == 1 && ammoInClipSlot1 > 0) || (activeSlot == 2 && ammoInClipSlot2 > 0))
                {
                    StartCoroutine(onShoot());
                    ReadyToShoot = false;
                }

            }
        }


        if (reloading == false)
        {
            checkReload();
        }
        if (Input.GetButtonDown("Reload") && ((activeSlot==1 && ammoInClipSlot1<clipSize)|| (activeSlot == 2 && ammoInClipSlot2 < clipSize)))
        {
            StartCoroutine(reload());
        }
         
            
	}
}
