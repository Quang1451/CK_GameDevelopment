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
}
