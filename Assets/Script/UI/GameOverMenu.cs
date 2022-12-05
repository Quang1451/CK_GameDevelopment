using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private LoadingScreenBarSystem load;

    void Awake() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Reset() {
        string json = File.ReadAllText(Application.dataPath+"/Player.json");
        SaveData value = JsonUtility.FromJson<SaveData>(json);
        PlayerDataSetting.instance.GetLoadData(value);
        load.loadingScreen(PlayerDataSetting.instance.Map);
    }

    public void Menu() {
        load.loadingScreen(0);
    }
}
