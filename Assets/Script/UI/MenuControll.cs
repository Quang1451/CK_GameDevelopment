using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MenuControll : MonoBehaviour
{
    [Header("Levels To Load")]
    public int newGameLevel;
    
    [SerializeField] private GameObject noSavedGameDialog = null;
    [SerializeField] private LoadingScreenBarSystem load;

    private string levelToLoad;
    
    public void NewGameDialogYes(){
        PlayerDataSetting.instance.DefaultData();

        SaveData save = new SaveData();
        save.Health =  PlayerDataSetting.instance.Health;
        save.RifleAmmo = PlayerDataSetting.instance.RifleAmmo;
        save.ShotgunAmmo = PlayerDataSetting.instance.ShotgunAmmo;
        save.SubmachineAmmo= PlayerDataSetting.instance.SubmachineAmmo;
        save.PistolAmmo = PlayerDataSetting.instance.PistolAmmo;
        save.Grenade = PlayerDataSetting.instance.Grenade;
        save.timePickupGun = PlayerDataSetting.instance.timePickupGun;
        save.Map = PlayerDataSetting.instance.Map;
        save.GunsHaving = PlayerDataSetting.instance.GunsHaving;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.dataPath+"/Player.json",json);

        load.loadingScreen(newGameLevel);
    }

    public void LoadGameDialogYes(){
        if(System.IO.File.Exists(Application.dataPath+"/Player.json")){
            string json = File.ReadAllText(Application.dataPath+"/Player.json");
            SaveData value = JsonUtility.FromJson<SaveData>(json);
            PlayerDataSetting.instance.GetLoadData(value);
            load.loadingScreen(PlayerDataSetting.instance.Map);
        }
        else{
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton(){
        Application.Quit();
    }
}
