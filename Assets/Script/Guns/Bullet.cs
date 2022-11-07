using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Data")]
    public float lifeBullet = 3;
    public float lifeHole = 8;
    public int minDamage;
    public int maxDamage;

    [Header("Reference")]
    public GameObject bulletHole;  
    public GameObject bloodHole;
    void Awake()
    {   
        Destroy(gameObject,lifeBullet);
    }

    //Gây dame và để lại dấu lổ đạn
    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == 7){
            if(bulletHole != null) {
                GameObject currentHole = Instantiate(bulletHole, this.transform.position, this.transform.rotation);
                Destroy(currentHole,lifeHole);
            }
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Enemy") {
            Enemy zb = collision.gameObject.GetComponent<Enemy>();
            if(zb != null) {
                zb.LoseHeal(Random.Range(minDamage, maxDamage));
                if(bloodHole != null) {
                GameObject currentHole = Instantiate(bloodHole, this.transform.position, this.transform.rotation);
                Destroy(currentHole,0.2f);
                }
                Destroy(gameObject);
            }
        }
        
        if(collision.gameObject.tag == "ExplosionItem") {
            ExplosionItem ei = collision.gameObject.GetComponent<ExplosionItem>();
            if(ei != null) {
                ei.LoseHeal(75);
            }
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Pickup") {
            Destroy(gameObject);
        }
    }

}