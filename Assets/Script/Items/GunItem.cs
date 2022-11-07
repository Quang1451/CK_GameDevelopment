using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    public string nameObject;
    public int ammoLeft;

    public int AmmoLeft {
        get {return ammoLeft;}
        set {ammoLeft = value;}
    }
}
