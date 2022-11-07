using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour
{
    NavMeshAgent nm;
    Transform target;
    Animator anim;
    bool isDead = false;
    public int damage_2_Player = 10;
    public bool attacking = false;
    [SerializeField]
    float chaseDistance = 2.25f;
    [SerializeField]
    float turnSpeed = 5f;
    [SerializeField]
    float attackTime = 3.033f;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        nm = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance >= chaseDistance && !isDead && !attacking){
            ChasePlayer();
        }
        else if (!attacking){
            AttackPlayer();
        }

        if (attacking){
            //make the zombie look at player while attacking
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        }
    }

    public void EnemyDeathAnimation(){
        isDead = true;
        anim.SetTrigger("Dead");
    }

    void ChasePlayer(){
        nm.updatePosition = true;
        nm.SetDestination(target.position);
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
    }

    void AttackPlayer(){
        anim.SetBool("isAttacking", true);
        anim.SetBool("isWalking", false);
        nm.updatePosition = false;
        nm.updateRotation = true;

        StartCoroutine(AttackTime());
    }

    public void DisableEnemy(){
        attacking = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    IEnumerator AttackTime(){
        attacking = true;

        yield return new WaitForSeconds(attackTime);
        //PlayerHealth.singleton.DamagePlayer(damage_2_Player);

        attacking = false;
    }
}
