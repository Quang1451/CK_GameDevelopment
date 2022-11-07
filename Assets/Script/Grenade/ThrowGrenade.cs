using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    private Animator animator;

    [Header("Reference")]
    public GameObject grenade;
    public GunsInventory data_script;

    [Header("Audio")]
    public AudioClip[] throwSounds;
	public AudioSource audioSource;

    [Header("Throw Data")]
    public float throwForce;
    
    private bool readyToThrow;

    void Start() {
        readyToThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject inventory = GameObject.Find("GunsInventory");
        for(int i = 0; i < inventory.transform.childCount; i++ ){
            if(inventory.transform.GetChild(i).gameObject.activeSelf == true) {
                animator = inventory.transform.GetChild(i).GetComponent<Animator>();
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && readyToThrow && data_script.CheckHasGrenade()) {
            audioSource.clip = throwSounds[Random.Range(0,throwSounds.Length)];
            audioSource.Play();
            Invoke("Throw",0.5f);
            readyToThrow = false;
        }        
    }

    void Throw() {
        animator.Play("Grenade Throw",0,0.0f);
        GameObject currentGrenade = Instantiate(grenade, transform.position, transform.rotation);
        currentGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        data_script.LoseGrenade();
        Invoke("ResetThrow",1);
    }

    void ResetThrow() {
        readyToThrow = true;
    }
}
