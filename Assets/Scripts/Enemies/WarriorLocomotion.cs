using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;

public class WarriorLocomotion : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;
    [SerializeField] WanderingAI randomMovement;
    [SerializeField] WarriorSword sword;

    public PlayerStats currentTarget;
    public LayerMask detectionLayer;

    public float distanceFromTarget;
    private float chaseRange = 10f;                     // when target is closer than this, chase!
    private float attackingDistance = 3f;

    private enum EnemyState { WALKING, CHASE, ATTACK };
    private EnemyState state;

    public bool canAttack = true;

    private void SetState(EnemyState newState, bool debugging = false)
    {
        if (debugging)
            print("newState: " + newState);
        state = newState;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(EnemyState.WALKING);
        sword.gameObject.GetComponent<MeshCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        animator.SetFloat("distance", distanceFromTarget);

        switch (state)
        {
            case EnemyState.WALKING: Update_Walking(); break;
            case EnemyState.CHASE: Update_Chase(); break;
            case EnemyState.ATTACK: Update_Attack(); break;
            default: Debug.Log("Invalid state!"); break;
        }
    }

    void Update_Walking()
    {
        navMeshAgent.isStopped = true;                             // stop the agent (following)
        animator.SetBool("isWalking", true);
        randomMovement.enabled = true;
        if (distanceFromTarget <= chaseRange)
        {
            animator.SetBool("isWalking", false);
            randomMovement.enabled = false;
            SetState(EnemyState.CHASE);
        }
    }

    void Update_Chase()
    {
        navMeshAgent.isStopped = false;                            // start the agent (following)
        navMeshAgent.SetDestination(currentTarget.transform.position);    // follow the target
        animator.SetBool("isChasing", true);
        if (distanceFromTarget > chaseRange)
        {
            animator.SetBool("isChasing", false);
            SetState(EnemyState.WALKING);
        }

        if (distanceFromTarget <= attackingDistance)
        {
            animator.SetBool("isChasing", false);
            SetState(EnemyState.ATTACK);
        }
    }

    void Update_Attack()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("isAttacking", true);
        if (distanceFromTarget > attackingDistance)
        {
            animator.SetBool("isAttacking", false);
            SetState(EnemyState.CHASE);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);  // draw a circle to show chase range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingDistance);  // draw a circle to show chase range
    }

    void OnAllowAttack()
    {
        StartCoroutine("ToggleSwordCollider");
    }

    IEnumerator ToggleSwordCollider()
    {
        sword.gameObject.GetComponent<MeshCollider>().enabled = true;
        yield return new WaitForSeconds(0.7f);
        sword.gameObject.GetComponent<MeshCollider>().enabled = false;
    }
}
