using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int enemyHealth = 100;
    ZombieAI enemyAI;

    private void Start(){
        enemyAI = GetComponent<ZombieAI>();
    }
    public void LoseHeal(int damage){
        enemyHealth -= damage;

        if(enemyHealth <= 0) {
            EnemyDead();
        }
    }

    void EnemyDead(){
        enemyAI.EnemyDeathAnimation();
        Destroy(gameObject,10);
    }
}
