using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Reference")]
    public GameObject flashlight;
    private bool FlashlightActive;
    // Start is called before the first frame update
    void Awake()
    {
        flashlight.SetActive(false);
        FlashlightActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            FlashlightActive = !FlashlightActive;
            flashlight.SetActive(FlashlightActive);
        }
    }
}
