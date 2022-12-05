using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    Animator animator;
    [Header("Volume Setting")]
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeTextValue = null;
    private float settingVolume;
    

    [Header("Graphics Setting")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle reesolutionToggle;
    private bool isFullScreen;
    int currentResolution;
    Resolution[] resolutions;

    void Start() {
        animator = GetComponent<Animator>();
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach(Resolution rslt in resolutions) {
            string option = rslt.width + "x" + rslt.height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }

    void OnEnable() {
        volumeSlider.value = AudioManager.instance.volume;
        volumeTextValue.text = AudioManager.instance.volume.ToString("0.00");
        resolutionDropdown.value = ScreenManager.instance.optionScreen;
        reesolutionToggle.isOn = ScreenManager.instance.isFullScreen;
    }

    public void ChangAnimation(int valueChange){
        if(valueChange == 0){
            animator.CrossFade("Option", 0.5f,0);
        }
        if(valueChange == 1){
            animator.CrossFade("SoundButtonClick", 0.5f,0);
            volumeSlider.value = AudioManager.instance.volume;
            volumeTextValue.text = AudioManager.instance.volume.ToString("0.00");

        }
        if(valueChange == 2){
            animator.CrossFade("GraphicsButtonClick", 0.5f,0);
            resolutionDropdown.value = ScreenManager.instance.optionScreen;
            reesolutionToggle.isOn = ScreenManager.instance.isFullScreen;
        }
        if(valueChange == 3){
            animator.CrossFade("GamePlayButtonClick", 0.5f,0);
        }
    }

    public void OnExitButtonClick(){
        animator.SetTrigger("ExitButtonClick");
    }

    public void DisbleThisGameObject(){
        gameObject.SetActive(false);
    }
    
    //Điều chỉnh âm thanh
    public void SetVolume(float volume)
    {
        settingVolume = volume;
        volumeTextValue.text = volume.ToString("0.00");
    }
    //Đặt âm thanh đã điều chỉnh
    public void VolumeApply(){
        AudioManager.instance.volume = settingVolume;
    }

    //Điều chỉnh đồ họa
    public void SetResolution(int option) {
        currentResolution = option;
    }

    public void SetDefaultGraphic() {
        isFullScreen = ScreenManager.instance.DefaulFullScreen;
        currentResolution = ScreenManager.instance.DefalultoptionScreen;
        reesolutionToggle.isOn = isFullScreen;
        resolutionDropdown.value = currentResolution;
    }

    public void GracphicApply() {
        isFullScreen = reesolutionToggle.isOn;
        ScreenManager.instance.isFullScreen = isFullScreen;
        ScreenManager.instance.optionScreen = currentResolution;
        Screen.SetResolution(resolutions[currentResolution].width, resolutions[currentResolution].height, ScreenManager.instance.isFullScreen);
    }
}
