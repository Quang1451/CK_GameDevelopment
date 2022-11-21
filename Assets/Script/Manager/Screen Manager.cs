using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Screen Manager", menuName="ScriptableObject/Screen Manager")]
public class ScreenManager :  SingletonScriptableObject<ScreenManager>
{
    [Range(0,21)]
    public int optionScreen = 21;
    public bool isFullScreen = true;
    
    [Header("Default")]
    public bool DefaulFullScreen = true;
    public int DefalultoptionScreen = 21;
}
