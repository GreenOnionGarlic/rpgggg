using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NewBehaviourScript : Entity
{
    bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDetected; 
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 2f * facingDir, rb.velocity.y);
                isAttacking = false;
                Debug.Log("I see the player");

            }
            else
            {
                Debug.Log("Attack");
                isAttacking = true;
            }
        }
        if (!isGrounded || isWallDetected)
        {
            Flip();
        }
        Movement();
    }

    private void Movement()
    {
        if (!isAttacking)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }
}
