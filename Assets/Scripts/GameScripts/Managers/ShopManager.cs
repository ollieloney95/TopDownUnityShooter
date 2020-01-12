using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    //public int ammo { get; private set; }

    // Use this for initialization
    public void Startup()
    {
        Debug.Log("Shop manager starting...");
        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onBuyPistol()
    {
        Managers.gun.equipGun("Pistol");
        Debug.Log("bought pistol");
        if (Managers.gun.slot1=="Pistol" || Managers.gun.slot2 == "Pistol") {
            AddAmmo("Pistol");
        }
    }
    public void onBuySMG()
    {
        Managers.gun.equipGun("SMG");
        Debug.Log("SMG");
        if (Managers.gun.slot1 == "SMG" || Managers.gun.slot2 == "SMG")
        {
            AddAmmo("SMG");
        }
    }
    public void onBuyLMG()
    {
        Managers.gun.equipGun("LMG");
        if (Managers.gun.slot1 == "LMG" || Managers.gun.slot2 == "LMG")
        {
            AddAmmo("LMG");
        }
    }
    public void onBuyRifle()
    {
        Managers.gun.equipGun("Rifle");
        if (Managers.gun.slot1 == "Rifle" || Managers.gun.slot2 == "Rifle")
        {
            AddAmmo("Rifle");
        }
    }
    public void onBuySniper()
    {
        Managers.gun.equipGun("Sniper");
        if (Managers.gun.slot1 == "Sniper" || Managers.gun.slot2 == "Sniper")
        {
            AddAmmo("Sniper");
        }
    }
    public void onBuyShotgun()
    {
        Managers.gun.equipGun("Shotgun");
        if (Managers.gun.slot1 == "Shotgun" || Managers.gun.slot2 == "Shotgun")
        {
            AddAmmo("Shotgun");
        }
    }
    public void onBuyPerkFireRate(){Managers.player.levelUpPerkFireRate();}
    public void onBuyPerkMaxAmmo() { Managers.player.levelUpPerkMaxAmmo(); }
    public void onBuyPerkClipSize() { Managers.player.levelUpPerkClipSize(); }
    public void onBuyPerkReloadSpeed() { Managers.player.levelUpPerkReloadSpeed(); }
    public void onBuyPerkSpeed() { Managers.player.levelUpPerkSpeed(); }
    public void onBuyPerkPower() { Managers.player.levelUpPerkPower(); }
    public void onBuyPerkHealth() { Managers.player.levelUpPerkHealth(); }

    void AddAmmo(string weapon)
    {
        if(Managers.gun.slot1 == weapon) { Managers.gun.ammoSlot1 = Managers.gun.maxAmmoList[weapon]; }
        if (Managers.gun.slot2 == weapon) { Managers.gun.ammoSlot2 = Managers.gun.maxAmmoList[weapon]; }
    }
}
