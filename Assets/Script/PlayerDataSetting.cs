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
    public GameObject[] GunsHaving;

    public GameObject[] GunsHavingDefault;

    public void DefaultData() {
        Health = 200;
        RifleAmmo = ShotgunAmmo = SubmachineAmmo = PistolAmmo = Grenade = 0;
        GunsHaving = GunsHavingDefault;
    }
}
