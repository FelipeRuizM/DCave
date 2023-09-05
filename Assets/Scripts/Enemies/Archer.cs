using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Archer : CharacterStats
{
    private int damageDivisor = 5;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 20;
        currentHealth = maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            GetDamaged((int)collision.impulse.magnitude / damageDivisor);
            Destroy(collision.gameObject);
            if (currentHealth < 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
