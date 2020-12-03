using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioAnimator : MonoBehaviour
{
    private bool onGround;
    private float speed;
    private Animator anim;
    private bool crouching;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        crouching = false;
    }
    
    void Update()
    {
        anim.SetBool("onGround", onGround);
        anim.SetFloat("speed", speed);
    }

    public void UpdateValues(bool onGround, float speed)
    {
        this.onGround = onGround;
        this.speed = speed;
    }

    public void TriggerJump(float jump)
    {
        anim.SetFloat("jumpCombo", jump);
        anim.SetTrigger("jump");
    }
    public void TriggerPunch(float punch)
    {
        anim.SetFloat("punchCombo", punch);
        anim.SetTrigger("punch");
    }

    public void Crouch(bool crouch)
    {
        if (crouch != crouching)
        {
            crouching = crouch;
            anim.SetBool("crouching", crouching);
        }
    }
}
