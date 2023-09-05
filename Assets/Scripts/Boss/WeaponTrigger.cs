using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    public bool hasCollide = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { }
        
    private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                if (hasCollide == false) { 
                    hasCollide = true;
                    playerStats.TakeDamage(2);
            }
        }    
    }
}
