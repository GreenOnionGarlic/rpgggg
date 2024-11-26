using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashtime;
    [SerializeField] private float dashduration;
    [SerializeField] private float dashspeed;
    [SerializeField] private float dashCoolDown;

    [Header("Attack info")]
    private bool isAttacking;
    private int comboCounter;
    [SerializeField] private float comboTime =  .8f;
    private float comboTimeWindow;






    private float xInput;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();
        FlipController();
        dashtime -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashtime < -dashCoolDown)
                dashtime = dashduration;
        }
        AnimatorControllers();
        
    }

    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;
        comboCounter = comboCounter++ % 3;



    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }



    private void CheckInput()
    {
        xInput = UnityEngine.Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startAttackEvent();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void startAttackEvent()
    {
        if (!isGrounded)
        {
            return;
        }
        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashtime > 0 && !isAttacking)
        {
            rb.velocity = new Vector2(facingDir * dashspeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * xInput, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
            
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetInteger("comboCounter", comboCounter);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDushing", dashtime > 0);
        anim.SetBool("isAttacking", isAttacking);
    }



        

    
    









}
