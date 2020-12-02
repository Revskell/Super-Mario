using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MarioAnimator : MonoBehaviour
{
    private bool onGround;
    private float speed;
    private float punchCombo;
    private float jumpCombo;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        anim.SetBool("onGround", onGround);
        anim.SetFloat("speed", speed);
        anim.SetFloat("punchCombo", punchCombo);
        anim.SetFloat("jumpCombo", jumpCombo);
    }

    public void UpdateValues(bool onGround, float speed, float punchCombo, float jumpCombo)
    {
        this.onGround = onGround;
        this.speed = speed;
        this.punchCombo = punchCombo;
        this.jumpCombo = jumpCombo;
    }

    public void TriggerJump() { anim.SetTrigger("jump"); }
    public void TriggerPunch() { anim.SetTrigger("punch"); }
}
