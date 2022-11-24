using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private LoadingScreenBarSystem load;

    public void Restart() {
        string json = File.ReadAllText(Application.dataPath+"/Player.json");
        SaveData value = JsonUtility.FromJson<SaveData>(json);
        PlayerDataSetting.Instance.GetLoadData(value);
        load.loadingScreen(PlayerDataSetting.Instance.Map);
    }

    public void Menu() {
        load.loadingScreen(0);
    }
}
