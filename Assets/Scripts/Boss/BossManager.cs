using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
   
    [SerializeField]
    BossLocomotion bossLocomotion;
    // Start is called before the first frame update
    public bool isPerformingAction;
    public float detectionRadius;
    public float maximumDetectionAngle = 50f;
    public float minimumDetectionAngle = -50f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        HandleCurrentAction();
    }
    public void HandleCurrentAction() {
        if (bossLocomotion.currentTarget == null)
        {
            bossLocomotion.HandleDetection();
        }
        else {
            bossLocomotion.HandleMoveToTarget();
        }
    }
   
}
