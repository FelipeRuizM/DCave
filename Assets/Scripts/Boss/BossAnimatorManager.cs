using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorManager : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayTargetAnimation(string targetAnim, bool isInteracting) { 
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
}
