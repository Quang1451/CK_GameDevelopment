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
        PlayerDataSetting.Instance.DefaultData();

        SaveData save = new SaveData();
        save.Health =  PlayerDataSetting.Instance.Health;
        save.RifleAmmo = PlayerDataSetting.Instance.RifleAmmo;
        save.ShotgunAmmo = PlayerDataSetting.Instance.ShotgunAmmo;
        save.SubmachineAmmo= PlayerDataSetting.Instance.SubmachineAmmo;
        save.PistolAmmo = PlayerDataSetting.Instance.PistolAmmo;
        save.Grenade = PlayerDataSetting.Instance.Grenade;
        save.timePickupGun = PlayerDataSetting.Instance.timePickupGun;
        save.Map = PlayerDataSetting.Instance.Map;
        save.GunsHaving = PlayerDataSetting.Instance.GunsHaving;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.dataPath+"/Player.json",json);

        load.loadingScreen(newGameLevel);
    }

    public void LoadGameDialogYes(){
        if(System.IO.File.Exists(Application.dataPath+"/Player.json")){
            string json = File.ReadAllText(Application.dataPath+"/Player.json");
            SaveData value = JsonUtility.FromJson<SaveData>(json);
            PlayerDataSetting.Instance.GetLoadData(value);
            load.loadingScreen(PlayerDataSetting.Instance.Map);
        }
        else{
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton(){
        Application.Quit();
    }
}
