using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T: SingletonScriptableObject<T>{
    private static T _instance;
    public static T Instance {
        get{
            if(_instance == null) {
                T[] assets = Resources.LoadAll<T>("");
                if(assets == null || assets.Length < 1) {
                    throw new System.Exception("Counld not find any singleton scriptable object instances in the resources.");
                }
                else if (assets.Length >1) {
                    Debug.LogWarning("Multiple instances of the singleton scriptable object found in the resrouces,");
                }
                _instance = assets[0]; 
            }
            return _instance;
        }
    }
}
