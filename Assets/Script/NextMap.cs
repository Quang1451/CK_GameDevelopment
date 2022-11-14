using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMap : MonoBehaviour
{
    [SerializeField] private LoadingScreenBarSystem load;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Player") {
            load.loadingScreen(1);
        }
    }
}
