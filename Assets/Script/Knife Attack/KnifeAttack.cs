using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{   
    [Header("Animation")]
    public Animator animator;
    
    [Header("Audio")]
    public AudioClip[] knifeSounds;
	public AudioSource audioSource;
    
    [Header("Knife Data")]
    public int damage;
    private bool readyToAttack, timeDamage, countinue;

    // Start is called before the first frame update
    void Start()
    {
        countinue = false;
        readyToAttack = true;
        timeDamage = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && readyToAttack) {
            if(countinue) {
                animator.Play("Knife Attack 1", 0, 0.0f);
                audioSource.clip = knifeSounds[1];
                audioSource.Play();
                removeContinueAttack();
                Invoke("hitDame",0.4f);
                CancelInvoke("removeContinueAttack");
                Invoke("resetAttack", 1.2f);
                Invoke("noHitDame", 1.2f);
            }
            else{
                animator.Play("Knife Attack 2", 0, 0.0f);
                audioSource.clip = knifeSounds[0];
                audioSource.Play();
                hitDame();
                countinue = true;
                Invoke("removeContinueAttack", 0.6f);
                Invoke("resetAttack", 0.4f);
                Invoke("noHitDame", 0.4f);
            }
            readyToAttack = false;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Enemy" && timeDamage){
            EnemyHealth zb = collision.gameObject.GetComponent<EnemyHealth>();
            if(zb != null) {
                zb.LoseHeal(damage);
            }

        }

        if(collision.gameObject.tag == "ExplosionItem") {
            ExplosionItem ei = collision.gameObject.GetComponent<ExplosionItem>();
            if(ei != null) {
                ei.LoseHeal(35);
            }
        }
    }

    private void removeContinueAttack() {
        countinue = false;
    }
    
    private void resetAttack() {
        readyToAttack = true;
    }

    private void hitDame() {
        timeDamage = true;
    }

    private void noHitDame() {
        timeDamage = false;
    }
}