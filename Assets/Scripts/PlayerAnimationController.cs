using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public bool isIdle, isWalking, isJumping, isStun;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("idle", isIdle);
        animator.SetBool("walk", isWalking);
        animator.SetBool("jump", isJumping);
        animator.SetBool("stun", isStun);
        //animator.SetTrigger("idle");
    }
}
