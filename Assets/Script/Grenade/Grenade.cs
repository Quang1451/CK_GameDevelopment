using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Data")]
    public float delay;
    public float radius;
    public float forcedExplosion;
    public int damage;

    [Header("Reference")]
    public GameObject explosionEffect;

    private float countdown;
    private bool hasExploded;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        hasExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded) {
            Explode();
            hasExploded  = true;
        }        
    }

    void Explode() {
        //Thể hiện hiệu ứng nổ
        Instantiate(explosionEffect, transform.position, Quaternion.identity * Quaternion.Euler(90f,0f,0f));
        
        //Lấy những vật thể trong hình cầu có bán kinh radius;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        //Gây sát thương cho các vật thể đã lấy
        foreach(Collider col in colliders) {
            if(col.tag == "Enemy") {
                //Trừ máu enemy
                EnemyHealth zb = col.GetComponent<EnemyHealth>();
                if(zb != null) {
                    zb.LoseHeal(damage);
                }    

                //Đẩy enemy
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if(rb != null) {
                    rb.AddExplosionForce(forcedExplosion, transform.position, radius);
                }
            }

            if(col.tag== "ExplosionItem") {
                ExplosionItem ei = col.GetComponent<ExplosionItem>();
                if(ei != null) {
                    ei.LoseHeal(200);
                }
            }
        }

        //Xóa lựu đạn
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Enemy"){
            Explode();
        }
    }
}
