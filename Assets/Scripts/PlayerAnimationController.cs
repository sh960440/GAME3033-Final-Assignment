using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public bool isRunning, isRunningLeft, isRunningRight, isJumping, won, lost;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("RunLeft", isRunningLeft);
        animator.SetBool("RunRight", isRunningRight);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("Win", won);
        animator.SetBool("Lose", lost);
    }
}
