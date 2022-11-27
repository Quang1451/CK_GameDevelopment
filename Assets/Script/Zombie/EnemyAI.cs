
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum AIStates{
        Idle,
        Wandering,
        Chasing,
        Attack,
        Dead
    }

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float speed;
    [SerializeField] private float speedRotate;
    [SerializeField] private float waitTime;
    [SerializeField] private float resetAttack;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private int dameAttack;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Mesh[] skin;
    [SerializeField] private SkinnedMeshRenderer meshRender;

    private EnemyHealth enemyHealth;
    private Animator animator;
    private AIStates currentState = AIStates.Idle;
    private Vector3 destination = Vector3.zero;

    private NavMeshAgent agent;
    public Vector3 target;

    float m_WaitTime =-1;
    float m_ResetAttack =-1;

    bool isDead, attacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        
        animator = GetComponent<Animator>();

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsDead", false);

        if(skin.Length >0) {
            meshRender.sharedMesh = skin[Random.Range(0,skin.Length - 1)];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;

        //Kiểm tra enemy hết máu hay không
        if(enemyHealth.checkEnemyDead()) {
            currentState = AIStates.Dead;
            isDead = true;
        }
        else {
            //Kiểm tra nếu player trong vùng chasing của zombie
            float distance = Vector3.Distance(transform.position, target);
            if (distance <= chaseDistance && distance > attackDistance && !isDead){
                currentState = AIStates.Chasing;
            }
            else if (distance <= attackDistance && !isDead) {
                currentState = AIStates.Attack;
            }
        }

        switch(currentState) {
            case AIStates.Idle:
                DoIdle();
                break;
            case AIStates.Wandering:
                DoWandering();
                break;
            case AIStates.Chasing:
                DoChasing();
                break;
            case AIStates.Attack:
                DoAttack();
                break;
            case AIStates.Dead:
                DoDead();
                break;
        }
    }

    void DoIdle() {
        //Kiểm tra đi đến điểm random hay
        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z)) < 0.5f) {
            animator.SetBool("IsWalking", false);
            m_WaitTime -= Time.deltaTime;
        }
        
        if(m_WaitTime > 0){
            return;
        }
        
        m_WaitTime = Random.Range(8.0f,10.0f);
        currentState = AIStates.Wandering;
    }

    void DoWandering() {
        if(agent.pathStatus != NavMeshPathStatus.PathComplete) {
            return;
        }
       
        animator.SetBool("IsWalking", true);
        destination = RandomNav();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination), speedRotate * Time.deltaTime);
        agent.SetDestination(destination);
        currentState = AIStates.Idle; 
    }

    void DoChasing() {
        animator.SetBool("IsWalking", true);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), speedRotate * Time.deltaTime);
        agent.SetDestination(target);
        agent.updatePosition = true;
        
    }

    void DoAttack() {
        if(m_ResetAttack < 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), speedRotate * Time.deltaTime);
            animator.Play("Zombie_Punch");
            Invoke("CheckAttackRange", 1);
        
            m_ResetAttack = resetAttack;
            agent.updatePosition = true;
            currentState = AIStates.Chasing;
        }
        else {
            m_ResetAttack -= Time.deltaTime;
        }
    }

    //Kiểm tra player ở trong tầm đánh của zombie hay không và gây dame
    void CheckAttackRange() {
        Collider[] hit = Physics.OverlapSphere(attackPoint.position, 1.5f);
        foreach(Collider col in hit) {
            if(col.gameObject.tag == "Player") {
                PlayerData player = col.GetComponent<PlayerData>();
                player.TakeDamage(Random.Range(dameAttack - 10, dameAttack + 10));
            }
        }
    }
    void DoDead() {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsDead", true);
        agent.SetDestination(transform.position);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject,10);
    }

    Vector3 RandomNav() {
        Vector3 randomDestination = transform.position +  UnityEngine.Random.insideUnitSphere * 10.0f;
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position,(randomDestination - transform.position).normalized, out hit, 1.0f,groundLayer)){

            return RandomNav();
        }

        return randomDestination;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, 1.5f);
    }
}
