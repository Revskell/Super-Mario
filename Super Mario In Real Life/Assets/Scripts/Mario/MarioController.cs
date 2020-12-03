using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarioAnimator))]
public class MarioController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField, Range(1f, 5f)] private float walkingSpeed = 3f;
    [SerializeField, Range(1f, 3f)] private float sprintAmount = 2f;
    [SerializeField, Range(0f, 1f)] private float crouchControl = 0.5f;
    [SerializeField, Range(0f, 1f)] private float airControl = 0.35f;
    [Header("Jumping")]
    [SerializeField, Range(0f, 20f)] private float jumpPower = 10f;
    [SerializeField, Range(0f, 1f)] private float multiJumpMargin = 0.25f;
    [SerializeField, Range(0f, 1f)] private float multiJumpMultiplier = 0.25f;
    [SerializeField, Range(1f, 5f)] private float fallingMultiplier = 2f;

    private List<Collider> grounds;
    private MarioAnimator animator;
    private MarioSounds sounds;
    private Rigidbody body;
    private Transform cam;
    private float jumpCounter;
    private int consecutiveJumps;
    private bool crouching;
    private bool sprinting;
    private bool toJump;
    private bool toPunch;
    private int punchCombo;
    private float punchCounter;
    private float basicMass;
    private bool onGround;
    private Vector3 input;
    private Collider feet;

    void Start()
    {
        grounds = new List<Collider>();
        animator = GetComponent<MarioAnimator>();
        sounds = GetComponentInChildren<MarioSounds>();
        body = GetComponent<Rigidbody>();
        input = new Vector3();
        foreach(Collider c in GetComponents<Collider>())
        {
            if (c.isTrigger)
            {
                feet = c;
                break;
            }
        }
        cam = Camera.main.transform;
        basicMass = body.mass;
        consecutiveJumps = 1;
        punchCombo = 1;
        punchCounter = 0f;
        jumpCounter = 0f;
        crouching = false;
        onGround = false;
        sprinting = false;
        animator.UpdateValues(onGround, 0f);
    }
    
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        //todo wall jumps
        if (onGround)
        {
            GetHeavy(false);
            if (toJump) Jump();
            else if (toPunch) Punch();
            else Move();
        }
        else
        {
            AirMove();
            if (body.velocity.y < 0f) GetHeavy(true);
        }
        animator.UpdateValues(onGround, body.velocity.magnitude);
    }

    private void Move()
    {
        Vector3 movement = (crouching ? crouchControl : (sprinting ? sprintAmount : 1f)) * walkingSpeed * (cam.forward.normalized * input.z + cam.right.normalized * input.x);
        movement.y = body.velocity.y;
        body.velocity = movement;
        if (input.magnitude >= Time.deltaTime) body.MoveRotation(Quaternion.Euler(0f, cam.localEulerAngles.y, 0f) * Quaternion.LookRotation(input, Vector3.up));
    }
    private void AirMove()
    {
        Vector3 movement = airControl * (sprinting ? sprintAmount : 1f) * walkingSpeed * (cam.forward.normalized * input.z + cam.right.normalized * input.x);
        movement.y = 0;
        body.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
        if (input.magnitude >= Time.deltaTime) body.MoveRotation(Quaternion.Euler(0f, cam.localEulerAngles.y, 0f) * Quaternion.LookRotation(input, Vector3.up));
    }

    private void GetInput()
    {
        OnGround();
        crouching = onGround && Input.GetButton("Crouch");
        animator.Crouch(crouching);
        sprinting = Input.GetButton("Sprint");
        if (punchCounter > 0f) punchCounter -= Time.deltaTime;
        else punchCombo = 1;
        input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (onGround)
        {
            if (jumpCounter > 0f) jumpCounter -= Time.deltaTime;
            else consecutiveJumps = 1;
            if (Input.GetButtonDown("Fire1")) toPunch = true;
            else if (Input.GetButtonDown("Jump")) toJump = true;
        }
    }

    private void GetHeavy(bool heavy) { body.mass = basicMass * (heavy ? fallingMultiplier : 1f); }

    private void Jump()
    {
        feet.enabled = false;
        onGround = false;
        grounds.Clear();
        Invoke("ActivateFeet", Time.deltaTime * 10f);
        if (crouching) ImpulseJump(Vector3.up * jumpPower + transform.forward * jumpPower, -1);
        else
        {
            if (jumpCounter > 0f && consecutiveJumps < 3) consecutiveJumps++;
            else consecutiveJumps = 1;
            ImpulseJump(Vector3.up * jumpPower * (1f + (consecutiveJumps - 1) * multiJumpMultiplier), consecutiveJumps);
            jumpCounter = multiJumpMargin;
        }
        toJump = false;
        crouching = false;
    }
    private void ActivateFeet() { feet.enabled = true; }

    private void ImpulseJump(Vector3 force, float jumpType)
    {
        animator.TriggerJump(jumpType);
        body.AddForce(force, ForceMode.Impulse);
    }

    private void Punch()
    {
        toPunch = false;
        if (punchCounter > 0f)
        {
            if (punchCombo < 3) punchCombo++;
            else punchCombo = 1;
        }
        punchCounter = 1f;
        animator.TriggerPunch(punchCombo);
        //todo actually punch
    }

    private void OnGround() { onGround = grounds.Count > 0; }
    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Ground")) grounds.Add(other); }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Ground")) grounds.Remove(other); }
}
