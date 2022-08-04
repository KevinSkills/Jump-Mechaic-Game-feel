using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public int playerIndex;

    KeyCode up, down, right, left;

    [Header("Rigidbody stuff")]
    float velX;
    float velY;

    [Header("Side Move")]
    public float acc;
    public float drag;
    Rigidbody2D rb;

    public LayerMask groundCheckLayerMask;

    float jumpKeyDownTime = -10;
    float jumpKeyReleaseTime = -10;
    float lastGroundTime = -10;
    float firstGroundTime = -10;
    float jumpTime = -10;

    bool isShortJump;
    bool tapped;

    bool onGround;

    [Header("Jumping")]
    bool isJumping;
    public float jumpForce;
    public float shortJumpThreshhold = 0.2f;
    public float shortJumpDivider;
    public float gravityScale = 80;
    float gravity;
    public float forniteMP = 1.25f;
    public float coyoteDelay = 0.3f;
    public float jumpKeyDelay = 0.3f;
    public float antiCoyoteDelay = 0.05f;
    public UnityEvent landEvent;

    public Transform groundCheckLocation;




    // Start is called before the first frame update
    void Start()
    {
        gravity = gravityScale;
        up = PlayerMeta.defs[playerIndex].up;
        down = PlayerMeta.defs[playerIndex].down;
        left = PlayerMeta.defs[playerIndex].left;
        right = PlayerMeta.defs[playerIndex].right;
        GetComponent<SpriteRenderer>().color = PlayerMeta.defs[playerIndex].c;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(up))
        {
            //gravity = gravityScale/2;
            jumpKeyDownTime = Time.time;
        }else if (Input.GetKeyUp(up))
        {
            //if(rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, 0);
            //if(isJumping) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpKeyReleaseTime = Time.time;
        }
        //if(rb.velocity.y <= -0.2f) gravity = gravityScale;
    }
    void FixedUpdate()
    {
        //-----PRECODE-----


        //Velocity varibles
        velX = rb.velocity.x;
        velY = rb.velocity.y;

        //-----CODE---------

        //for moving
        int horizontal = ((Input.GetKey(right)) ? 1 : 0) - ((Input.GetKey(left)) ? 1 : 0);
        velX += horizontal * acc * Time.fixedDeltaTime;
        velX *= Mathf.Pow(drag, Time.fixedDeltaTime);

        //for jumping
        
        if (GroundCheck() && almostNumber(velY, 0))
        {
            if (!onGround) Land();
            onGround = true;
            lastGroundTime = Time.time;
        }else {
            onGround = false;
        }
        if (!isJumping && antiCoyoteDelay < Time.time - firstGroundTime
            && Time.time - lastGroundTime < coyoteDelay
            && Time.time - jumpKeyDownTime < jumpKeyDelay) {
            //Now jumping
                isJumping = true;
                jumpTime = Time.time;
                velY = jumpForce;
        }

        //short jump
        if (!isShortJump && isJumping
            && jumpKeyReleaseTime - jumpKeyDownTime < shortJumpThreshhold //has be a tap
            && Time.time - jumpTime > shortJumpThreshhold //wait a bit if pressed in advance
            && jumpKeyDownTime < jumpKeyReleaseTime) {
                isShortJump = true;
                print("heheyhreyani");
                velY /= shortJumpDivider;
        }

        //gravity
        velY-= Time.fixedDeltaTime * ((velY < 0)
        ? forniteMP * gravity
        : gravity
        );


        // ------AFTER CODE-------
        rb.velocity = new Vector2(velX, velY );
    }

    bool GroundCheck() {
        Vector2 rectCenter = (Vector2)groundCheckLocation.position;
        return (Physics2D.OverlapArea(rectCenter + new Vector2(0.4f, 0.1f),
            rectCenter - new Vector2(0.4f, 0.1f),
            groundCheckLayerMask));
    }

    Collider2D GroundCheckCol()
    {
        Vector2 rectCenter = (Vector2)groundCheckLocation.position;
        return (Physics2D.OverlapArea(rectCenter + new Vector2(0.4f, 0.1f),
            rectCenter - new Vector2(0.4f, 0.1f),
            groundCheckLayerMask));
    }

    void Land()
    {
        isShortJump = false;
        isJumping = false;
        firstGroundTime = Time.time;
        landEvent.Invoke();
    }

    bool almostNumber(float input, float target)
    {
        if (Mathf.Abs(input - target) < 0.01) return true;
        return false;
    }
}