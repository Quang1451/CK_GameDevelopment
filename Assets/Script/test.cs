using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public PlayerData data;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) {
            data.TakeDamage(40);
        }        
    }
}
