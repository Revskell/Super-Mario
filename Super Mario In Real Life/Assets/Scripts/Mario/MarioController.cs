using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarioAnimator))]
public class MarioController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField, Range(1f, 5f)] private float walkingSpeed = 1f;
    [SerializeField, Range(1f, 3f)] private float sprintAmount = 2f;
    [SerializeField, Range(1f, 10f)] private float jumpHeight = 2f;
    [SerializeField, Range(1f, 10f)] private float jumpPower = 1f;

    [Header("Misc")]
    [SerializeField] private LayerMask marioMask = 0;

    private MarioAnimator animator;
    private Rigidbody body;
    private float jumpCounter;
    private float punchCounter;
    
    void Start()
    {
        animator = GetComponent<MarioAnimator>();
        body = GetComponent<Rigidbody>();
        animator.UpdateValues(false, 0f, 0f, 0f);
    }
    

    void Update()
    {
        if (jumpCounter > 0f) jumpCounter -= Time.deltaTime;
        else if (jumpCounter <= Time.deltaTime) jumpCounter += Time.deltaTime;
        if (punchCounter > 0f) punchCounter -= Time.deltaTime;

        bool onGround = OnGround();
        if (onGround)
        {
            if (Input.GetButtonDown("Fire1")) Punch();
            else if (Input.GetButtonDown("Jump")) Jump();
        }
        //todo calculate movement input
        //todo allow punch
        //todo allow jumps
        //todo allow sprint
        //todo move
        //todo update values
        //
        animator.UpdateValues(onGround, body.velocity.magnitude, punchCounter, jumpCounter);
    }

    private void Jump()
    {
        if (Input.GetButton("Crouch")) jumpCounter = -1f;
        else jumpCounter += 1f;
        animator.TriggerJump();
    }

    private void Punch()
    {
        punchCounter += 1f;
        animator.TriggerPunch();
    }

    private bool OnGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f, marioMask))
        {
            return hit.collider.gameObject.CompareTag("Ground");
        }
        return false;
    }
}
