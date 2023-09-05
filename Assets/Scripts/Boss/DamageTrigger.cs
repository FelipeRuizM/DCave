using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    BossStats bossStats;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            bossStats.TakeDamage(10);
        }
    }
}
