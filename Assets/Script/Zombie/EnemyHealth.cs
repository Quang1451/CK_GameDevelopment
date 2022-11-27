using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int enemyHealth = 100;

    public void LoseHeal(int damage){
        enemyHealth -= damage;
    }

    public bool checkEnemyDead() {
        if(enemyHealth <= 0)
            return true;
        return false;
    }
    
}
