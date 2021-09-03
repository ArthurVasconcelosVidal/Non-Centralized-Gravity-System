using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour{
    [SerializeField] Animator animator;

    public void CallJump(bool state) {
        animator.SetBool("Jump", state);
    }
    public void SetRunningAnim(float magnitude) {
        animator.SetFloat("Velocity", magnitude);
        //animator.blend
    }    
    
}
