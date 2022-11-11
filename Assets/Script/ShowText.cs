using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{   
    [SerializeField] private GameObject text;
    [SerializeField] private string content;
    [SerializeField] private float time;
    private bool show = false;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        if(!show && other.gameObject.name == "Player") {
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().text = content;
            show = true;
            Invoke("DisappearingText",time);
        }
        
    } 

    void DisappearingText() {
       text.SetActive(false); 
    }
    /* void OnTriggerExit(Collider other) {
        //text.SetActive(false);  
    } */
}
