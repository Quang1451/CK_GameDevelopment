using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public int currentHealth;
    public int maxHealth = 100;
    public bool isDead = false;

    public void Awake() {
        singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void DamagePlayer(int damage){
        if(currentHealth > 0){
            currentHealth -= damage;
        }
        else{
            Dead();
        }
    }
    void Dead(){
        currentHealth = 0;
        isDead = true;
        Debug.Log("Player Is Dead!");
    }
}
