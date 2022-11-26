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
    }

    [SerializeField] private float speed;
    [SerializeField] private float timeRotate;
    [SerializeField] private float viewRadius;
    [SerializeField] private float waitTime;
    [SerializeField] private float chaseDistance;
    [SerializeField] private LayerMask groundLayer;


    private Animator animator;
    private AIStates currentState = AIStates.Idle;

    private Vector3 playerLastPosition = Vector3.zero;
    private Vector3 destination = Vector3.zero;

    private NavMeshAgent agent;
    public Vector3 target;

    float m_WaitTime;
    
    bool m_PlayerInRange, m_PlayerNear, m_IsPatrol, m_CaughtPlayer;
    bool isDead, attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        m_IsPatrol = true;
        m_CaughtPlayer = m_PlayerNear = false;

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        
        animator = GetComponent<Animator>();

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttack", false);
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;

        float distance = Vector3.Distance(transform.position, target);
        if (distance <= chaseDistance && !isDead && !attacking){
            currentState = AIStates.Chasing;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination), timeRotate * Time.deltaTime);
        agent.SetDestination(destination);
        currentState = AIStates.Idle; 
    }

    void DoChasing() {
        animator.SetBool("IsWalking", true);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), timeRotate * Time.deltaTime);
        agent.updatePosition = true;
        agent.SetDestination(target);
    }

    Vector3 RandomNav() {
        Vector3 randomDestination = transform.position +  UnityEngine.Random.insideUnitSphere * 10.0f;
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position,(randomDestination - transform.position).normalized, out hit, 1.0f,groundLayer)){

            return RandomNav();
        }

        return randomDestination;
    }
}
