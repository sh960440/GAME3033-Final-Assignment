using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public bool isRunning, isRunningLeft, isRunningRight, isJumping, lost = false;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("RunLeft", isRunningLeft);
        animator.SetBool("RunRight", isRunningRight);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("Lose", lost);
    }
}
