using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public int currentSceneNumber;
    public int speedPerkLevel { get; private set; }
    public int healthPerkLevel { get; private set; }
    public int reloadSpeedPerkLevel { get; private set; }
    public int powerPerkLevel { get; private set; }
    public int fireRatePerkLevel { get; private set; }
    public int clipSizePerkLevel { get; private set; }
    public int maxAmmoPerkLevel { get; private set; }
    public int currentHealth { get; private set; }
    public int maxHealth;
    private float timeSinceDamage;
    private float regenTime = 3f;
    void Awake()
    {
        Messenger.AddListener(GameEvent.medipack, onMedipack);
        //Messenger.AddListener(GameEvent.ammopack, onAmmopack);
        maxHealth = 5 + 5 * healthPerkLevel;
		currentHealth = maxHealth;
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.medipack, onMedipack);
        //Messenger.RemoveListener(GameEvent.ammopack, onAmmopack);
    }
    void onMedipack()
    {
        currentHealth = maxHealth;
    }
    public void levelUpPerkSpeed()
    {
        speedPerkLevel += 1;
        if (speedPerkLevel > 5)
        {
            speedPerkLevel = 5;
        }
    }
    public void levelUpPerkHealth()
    {
        Debug.Log("up 1 health perk");
        healthPerkLevel += 1;
        if (healthPerkLevel > 5)
        { 
            healthPerkLevel = 5;
        }
        maxHealth = 5 + 5 * healthPerkLevel;
    }
    public void levelUpPerkReloadSpeed()
    {
        reloadSpeedPerkLevel += 1;
        if (reloadSpeedPerkLevel > 5)
        {
            reloadSpeedPerkLevel = 5;
        }
    }
    public void levelUpPerkPower()
    {
        powerPerkLevel += 1;
        if (powerPerkLevel > 5)
        {
            powerPerkLevel = 5;
        }
    }
    public void levelUpPerkClipSize()
    {
        clipSizePerkLevel += 1;
        if (clipSizePerkLevel > 5)
        {
            clipSizePerkLevel = 5;
        }
    }
    public void levelUpPerkMaxAmmo()
    {
        maxAmmoPerkLevel += 1;
        if (maxAmmoPerkLevel > 5)
        {
            maxAmmoPerkLevel = 5;
        }
    }
    public void levelUpPerkFireRate()
    {
        fireRatePerkLevel += 1;
        if (fireRatePerkLevel > 5)
        {
            fireRatePerkLevel = 5;
        }
    }
    // Use this for initialization
    public void Startup()
    {
        Debug.Log("Player manager starting...");
        status = ManagerStatus.Started;
    }
    public void changeHealth(int value)
    {
        timeSinceDamage = 0;
        currentHealth += value;
        if(currentHealth < 1)
        {
            onPlayerDeath();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    void onPlayerDeath()
    {
        //SceneManager.LoadScene("MainMenu");
        if(Managers.game.currentRound>Managers.stats.maxRound[currentSceneNumber-1]){
            Managers.stats.maxRound[currentSceneNumber-1] = Managers.game.currentRound;
        }
        GetComponent<UIController>().onDeath();
    }
    // Update is called once per frame
    void Update()
    {
        
        timeSinceDamage += Time.deltaTime;
        if(timeSinceDamage > regenTime)
        {
            currentHealth = maxHealth;
        }
    }
}