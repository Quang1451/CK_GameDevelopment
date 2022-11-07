using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionItem : MonoBehaviour
{
    [Header("Explosion Data")]
    public int health;
    public float radius;
    public float forcedExplosion;
    public int damageEnemy;
    public int damagePlayer;

    [Header("Reference")]
    public GameObject explosionEffect;

    private bool hasExploded;
    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasExploded) {
            Explode();
            hasExploded = true;
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
                Enemy zb = col.GetComponent<Enemy>();
                if(zb != null) {
                    zb.LoseHeal(damageEnemy);
                }    
                //Đẩy enemy
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if(rb != null) {
                    rb.AddExplosionForce(forcedExplosion, transform.position, radius);
                }
            }

            if(col.tag == "Player") {
                //Trừ máu Player
                PlayerData player = col.GetComponent<PlayerData>();
                if(player != null) {
                    player.TakeDamage(damagePlayer);
                }    
            }

            if(col.tag == "ExplosionItem") {
                //Cho nổ
                ExplosionItem ei = col.GetComponent<ExplosionItem>();
                if(ei != null) {
                    ei.LoseHeal(200);
                }    
            }
        }

        //Xóa lựu đạn
        Destroy(gameObject);
    }

    public void LoseHeal(int damage) {
        health -= damage;
    }
}
