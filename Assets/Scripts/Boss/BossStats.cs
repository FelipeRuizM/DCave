using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStats : CharacterStats
{
    [SerializeField] private AudioSource bossDamagedSound;
    // Start is called before the first frame update
    [SerializeField]
    NavMeshAgent navMeshAgent;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        //currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel() {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth = currentHealth - damage;
        // Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.Play("death");
            navMeshAgent.isStopped = true;
            Messenger.Broadcast("BOSS_DIED");
        }
        else {
            anim.SetTrigger("isHit");
            bossDamagedSound.Play();
            navMeshAgent.isStopped = true;
        }
    }
}
