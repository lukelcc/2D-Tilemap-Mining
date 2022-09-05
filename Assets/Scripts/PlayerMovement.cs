using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerRunSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbLadderSpeed = 5f;
    [SerializeField] float gravityScaleAtStart = 1f;
    [SerializeField] int maxJumps = 2;

    private Vector2 moveInput;

    //colliders
    //boxCollider = feet
    //capsuleCollider = body
    int jumpsAvailable = 0;

    //private bool canDash = true;
    //private bool isDashing = false;


    //[Header("Dash settings")]
    //[SerializeField] private float dashingPower = 20f;
    //[SerializeField] private float dashingTime = 0.2f;
    //[SerializeField] private float dashingCooldown = 1f;



    //private void OnEnable()
    //{
    //    GetComponent<_2Dplatformcontrols>().Enable();
    //}

    //private void OnDisable()
    //{
    //    GetComponent<_2Dplatformcontrols>().Disable();
    //}

    // Update is called once per frame
    void Update()
    {           
        Run();        
        ClimbLadder();
        FlipSprite(); //has to be last
    }

    public Vector2 getMoveInput()
    {
        return moveInput;
    }

    void OnMove(InputValue value) // getting WSAD key input from user
    {

        moveInput = value.Get<Vector2>();       
    }


    // Jump - put in separate script? // new input system
    void OnJump(InputValue value) // getting space key from user 
    {
        
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            jumpsAvailable = maxJumps;
        }


        if (value.isPressed && jumpsAvailable > 0) // jump in air?
        {
            jumpsAvailable--;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // prevent velocity stack frm previous jump
            GetComponent<Rigidbody2D>().velocity += new Vector2(0f, jumpSpeed);
        }
    }

    //private void flyUpAnims()
    //{
    //    if (GetComponent<Rigidbody2D>().velocity.y > 0)
    //    {
    //        GetComponent<Animator>().SetBool("isFlyingUp", true);
    //    }
    //}
    
    //private void fallDownAnims()
    //{
    //    if (GetComponent<Rigidbody2D>().velocity.y < 0)
    //    {
    //        GetComponent<Animator>().SetBool("isFallingDown", true);
    //    }
    //}

    void Run()
    {
        //set transformation
        //if (isDashing) return;
        Vector2 playerVelocity = new Vector2(moveInput.x*playerRunSpeed, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().velocity = playerVelocity;

        //change animation to running only if the player is moving left/right
        //bool playerIsMovingHorinzontally = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        //GetComponent<Animator>().SetBool("isRunning", playerIsMovingHorinzontally);
    }

    void FlipSprite()
    {
        bool playerIsMovingHorizontally = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        if (playerIsMovingHorizontally)
        {
            //transform.localScale = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x), 1f); //left=-1, right=1
            if (Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) < 0) //left=-1, 
                GetComponent<SpriteRenderer>().flipX = false;
            else if (Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) > 0) //right = 1
                GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    private void ClimbLadder()
    {
        //only can climb when player touches ladder
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //if not climbing, gravity maintain the same
            GetComponent<Rigidbody2D>().gravityScale = gravityScaleAtStart;
            //GetComponent<Animator>().SetBool("isClimbing", false);
            return;
        }
        //if climbing, set gravity to 0
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        //set transformation for climbing    
        Vector2 climbLadderVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, moveInput.y * climbLadderSpeed);
        GetComponent<Rigidbody2D>().velocity = climbLadderVelocity;

        //change animation to climbing only if the player is moving up/down
        bool playerHasVerticalSpeedWhenClimbing = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > Mathf.Epsilon;
        //GetComponent<Animator>().SetBool("isClimbing", playerHasVerticalSpeedWhenClimbing);

    }
}
