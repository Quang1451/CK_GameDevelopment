using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{       
    [Header("Camera Data")]
    public float sensX = 200f;
    public float sensY = 100f;
    public float moveSpeed = 5f;
    public float gravity = -10f;

    [Header("Player Data")]
    public float jumgHeight = 1f;
    public float groundDistance = 0.4f;
    public float normalHeight = 2f;
    public float crouchHeight = 0.6f;
    protected int maxHealth = 200;
    protected int health;

    [Header("Graphic")]
    public TextMeshProUGUI HealthDisplay;

    private bool isDead;

    void Awake() {
        health = maxHealth;
        isDead = false;
    }

    void Update() {
        //Hiển thị máu
        if(HealthDisplay != null)
            HealthDisplay.SetText("HP: "+health);
    }

    public void CheckHealth() {
        if (health <= 0) {
            health = 0;
            Die();
        }

        if ( health >= maxHealth) {
            health = maxHealth;
        }        
    }
 
    public void Die() {
        isDead = true;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        CheckHealth();   
    }

    public void Heal(int heal) {
        health += heal;
        CheckHealth();
    }

}
