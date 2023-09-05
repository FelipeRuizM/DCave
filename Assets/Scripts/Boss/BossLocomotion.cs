using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
public class BossLocomotion : MonoBehaviour
{
    [SerializeField]
    BossManager bossManager;
    [SerializeField]
    BossAnimatorManager bossAnimatorManager;
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    BossStats bossStats;
    [SerializeField]
    public Collider weaponCollider;
    [SerializeField]
    public Transform attackPosition;
    [SerializeField]
    public Transform defaultPosition;
    [SerializeField]
    private WeaponTrigger weaponTrigger;
    [SerializeField]
    private GameObject doorBoss;

    public PlayerStats currentTarget;
    public LayerMask detectionLayer;

    public float distanceFromTarget;
    private float chaseRange = 50f;                     // when target is closer than this, chase!
    private float attackingDistance = 3f;

    private enum EnemyState { IDLE, CHASE, ATTACK, DEAD };
    private EnemyState state;

    private bool playerTagged = false;

    private void SetState(EnemyState newState, bool debugging = false)
    {
        if (debugging)
            print("newState: " + newState);
        state = newState;
    }

   
    // Start is called before the first frame update
    void Start()
    {
        SetState(EnemyState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentTarget != null) {
        //    if (transform.position == currentTarget.transform.position) {
        //        bossAnimatorManager.anim.SetBool("isChasing", true);
        //    }
        //}
        if (playerTagged) {
            distanceFromTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
            // what happens here depends on the state we're currently in!
            if (bossStats.currentHealth <= 0)
            {
                Update_Dead();
            }
            else {
                switch (state)
                {
                    case EnemyState.IDLE: Update_Idle(); break;
                    case EnemyState.CHASE: Update_Chase(); break;
                    case EnemyState.ATTACK: Update_Attack(); break;
                    default: Debug.Log("Invalid state!"); break;
                }
            }
            
        }
    }

    public void HandleDetection()
    {
        //Debug.Log("Afyer hit");
        Collider[] colliders = Physics.OverlapSphere(transform.position, bossManager.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++) {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();
            //Debug.Log(playerStats.maxHealth);

            if (playerStats != null) {
                //Debug.Log("Found player");
                Vector3 targetDirection = playerStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                //Debug.Log(viewableAngle);

                if (viewableAngle > bossManager.minimumDetectionAngle && viewableAngle < bossManager.maximumDetectionAngle)
                {
                    //Debug.Log("Current target is now tagged");
                    currentTarget = playerStats;
                    
                }
            }
            // Debug.Log("Player not found");
        }

    }
    private void CloseBossDoor() {
        doorBoss.transform.rotation = Quaternion.Euler(new Vector3(0.0f,-90.0f,0.0f));
    }
    public void HandleMoveToTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);

        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        playerTagged = true;
        
    }

    void Update_Idle()
    {
        DisableWeaponCollider();
        bossAnimatorManager.anim.SetBool("isChasing", false);
        navMeshAgent.isStopped = true;                             // stop the agent (following)
        if (distanceFromTarget <= chaseRange)
        {
            SetState(EnemyState.CHASE);
        }
    }

    void Update_Chase()
    {
        DisableWeaponCollider();
        CloseBossDoor();

        bossAnimatorManager.anim.SetBool("isAttacking", false);
        bossAnimatorManager.anim.SetBool("isChasing", true);
       
        
        navMeshAgent.isStopped = false;                            // start the agent (following)
        navMeshAgent.SetDestination(currentTarget.transform.position);    // follow the target
        if (distanceFromTarget > chaseRange)
        {
            SetState(EnemyState.IDLE);
        }

        if (distanceFromTarget <= attackingDistance)
        {
            SetState(EnemyState.ATTACK);
        }
  
    }

    void Update_Attack()
    {
        
        EnableWeaponCollider();
        navMeshAgent.isStopped = true;                            // start the agent (following)
        
        bossAnimatorManager.anim.SetBool("isAttacking", true);
        if (distanceFromTarget > attackingDistance)
        {
            SetState(EnemyState.CHASE);
        }
    }

    
    void Update_Dead() {
        DisableWeaponCollider();
        bossAnimatorManager.anim.Play("death");
        navMeshAgent.isStopped = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);  // draw a circle to show chase range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingDistance);  // draw a circle to show chase range
    }

    // called by the animator event
    public void ChangeCollider()
    {
        EnableWeaponCollider();
        // Debug.Log("Collide transfer");
        if (weaponCollider != null && attackPosition != null)
        {
            weaponCollider.transform.position = attackPosition.position;
        }
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    public void ResetCollider() {
        DisableWeaponCollider();
        weaponCollider.transform.position = defaultPosition.position;
        weaponTrigger.hasCollide = false;
    }
}
