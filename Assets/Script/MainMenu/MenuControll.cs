using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuControll : MonoBehaviour
{
    [Header("Levels To Load")]
    public int newGameLevel;
    
    [SerializeField] private GameObject noSavedGameDialog = null;
    [SerializeField] private LoadingScreenBarSystem load;

    private string levelToLoad;
    
    public void NewGameDialogYes(){
        PlayerDataSetting.Instance.DefaultData();
        load.loadingScreen(newGameLevel);
    }

    public void LoadGameDialogYes(){
        if(PlayerPrefs.HasKey("SavedLevel")){
            levelToLoad = PlayerPrefs.GetString("SaveLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else{
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton(){
        Application.Quit();
    }
}
