using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T: ScriptableObject{
    private static T _instance;
    public static T Instance {
        get{
            if(_instance == null) {
                T[] assets = Resources.FindObjectsOfTypeAll<T>();
                if(assets.Length == 0) {
                    Debug.LogError("Counld not find any singleton scriptable object instances in the resources.");
                    return null;
                }
                else if (assets.Length >1) {
                    Debug.LogError("Multiple instances of the singleton scriptable object found in the resrouces,");
                    return null;
                }
                _instance = assets[0]; 
            }
            return _instance;
        }
    }
}
