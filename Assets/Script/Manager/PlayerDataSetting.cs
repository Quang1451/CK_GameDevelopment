using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Player Data Setting", menuName="ScriptableObject/Player Data Setting")]
public class PlayerDataSetting : SingletonScriptableObject<PlayerDataSetting>
{
    public int Health;
    public int RifleAmmo;
    public int ShotgunAmmo;
    public int SubmachineAmmo;
    public int PistolAmmo;
    public int Grenade;
    public int Map;
    public int timePickupGun;
    public GameObject[] GunsHaving;

    public void DefaultData() {
        Health = 200;
        RifleAmmo = ShotgunAmmo = SubmachineAmmo = PistolAmmo = Grenade = 0;
        GunsHaving[0] = Resources.Load<GameObject>("Guns/Glock");
        GunsHaving[1] = null;
        GunsHaving[2] = null; 
        timePickupGun= 0;
        Map = 1;
    }

    public void GetLoadData(SaveData loadData) {
        Health = loadData.Health;
        RifleAmmo = loadData.RifleAmmo;
        ShotgunAmmo = loadData.ShotgunAmmo;
        SubmachineAmmo = loadData.SubmachineAmmo;
        PistolAmmo = loadData.PistolAmmo;
        Map = loadData.Map;
        timePickupGun = loadData.timePickupGun;
        GunsHaving = loadData.GunsHaving;
    }
}
