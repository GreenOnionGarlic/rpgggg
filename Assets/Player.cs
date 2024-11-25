using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashtime;
    [SerializeField] private float dashduration;
    [SerializeField] private float dashspeed;

    [Header("Attack info")]
    private bool isAttacking;
    private int comboCounter;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrouded;

    private int facingDir = 1;
    private bool facingRight = true;

    private float xInput;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        CollisionCheck();
        dashtime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashtime = dashduration;
        }
        AnimatorControllers();
        FlipController();
    }

    public void AttackOver()
    {
        isAttacking = false;
    }

    private void CollisionCheck()
    {
        isGrouded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = UnityEngine.Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttacking = true;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Movement()
    {
        if (dashtime > 0)
        {
            rb.velocity = new Vector2(xInput * dashspeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * xInput, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrouded) 
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrouded);
        anim.SetBool("isDushing", dashtime > 0);
        anim.SetBool("isAttacking", isAttacking);
    }

    private void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
        

    
    









}
