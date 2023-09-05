using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private float delayToDestroyArrow = 10f;
    private int damageDivisor = 5; // small => more damage


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void Shoot(float speed)
    {
        this.GetComponent<BoxCollider>().enabled = true;
        if (rb)
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        StartCoroutine(DestroyAfterSeconds(delayToDestroyArrow));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool debugging = false;

        if (collision.gameObject.tag != "Arrow" && collision.gameObject.tag != "Player")
        {
            if (debugging) print(collision.gameObject.tag);
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            Destroy(this.gameObject.GetComponent<BoxCollider>());
        }
        if (collision.gameObject.tag == "Boss") {
            if (debugging) print((int)collision.impulse.magnitude / damageDivisor);
            collision.gameObject.GetComponent<BossStats>().TakeDamage((int)collision.impulse.magnitude  / damageDivisor);
            Destroy(this.gameObject);
        }
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    if (debugging) print((int)collision.impulse.magnitude / damageDivisor);
        //    collision.gameObject.GetComponent<CharacterStats>().GetDamaged((int)collision.impulse.magnitude / damageDivisor);
        //    Destroy(this.gameObject);
        //}
    }
}
