using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float jumpForce = 400f;
    [Range(0, 1)]
    [SerializeField]
    private float crouchSpeed = 0f;//.36f;  
    //[SerializeField]
    //private bool airControl = false;   
    [SerializeField]
    private LayerMask whatIsGround;

    private Collider2D[] colliders;
    private Transform groundCheck;
    const float groundedRadius = .2f;
    private bool grounded;
    private Transform ceilingCheck;
    const float ceilingRadius = .01f;
    //private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D rig;
    private bool facingRight = true;

    private DistanceJoint2D joint;
    private float fallMultiplier = 5f;
    private float lowJumpMultiplier = 4f;
    private float dashSpeed = 1f;
    private float dashSpeedMultiplier = 1.5f;
    [SerializeField]
    private LayerMask hookMask;
    /* private Transform rightCheck;
     private Transform leftCheck;*/
    private Vector3 startPoint;
    private Vector2 boxSize;
    [SerializeField]
    private float groundedDepth = 0.2f;
    Vector2 newBoxSize;

    private bool water;
    [SerializeField]
    private float waterMultiplier = 2f;
    private float jumpForceTemp;
    [SerializeField]
    private float dashDuration = 1f;
    private float dashTimer = 0f;
    private bool airDash = false;

    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        /*rightCheck = transform.Find("RightCheck");
        leftCheck = transform.Find("LeftCheck");*/
        startPoint = GameObject.Find("StartPoint").transform.position;

        //m_Anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        boxSize = new Vector2(GetComponent<BoxCollider2D>().size.x - .05f, groundedDepth);

        groundCheck.position = new Vector3(transform.position.x, (transform.position.y - rig.GetComponent<BoxCollider2D>().size.y / 2), transform.position.z);
        //ceilingCheck.position = new Vector3(transform.position.x, (transform.position.y + rig.GetComponent<SpriteRenderer>().size.y / 2), transform.position.z);

        joint.enabled = false;
        joint.distance = GetComponent<BoxCollider2D>().size.x / 2;
    }


    private void FixedUpdate()
    {
        grounded = false;

        //newBoxSize = new Vector2(boxSize.x, boxSize.y + (Mathf.Abs(rig.velocity.y) * groundedDepth * 2));
        colliders = Physics2D.OverlapBoxAll(groundCheck.position, boxSize, 0f, whatIsGround);  //OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }

        //m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        if (!water && dashSpeed != dashSpeedMultiplier)
        {
            if (rig.velocity.y < 0)
            {
                rig.gravityScale = fallMultiplier;
            }
            else if (rig.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rig.gravityScale = lowJumpMultiplier;
            }
            else
            {
                rig.gravityScale = 3f;
            }
        }
        else if (water)
        {
            rig.gravityScale = 3f;
        }
    }


    public void Move(float move, bool crouch)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)//&& m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround) && crouch)
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        //m_Anim.SetBool("Crouch", crouch);

        //move = (crouch ? move * crouchSpeed : move);

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        //m_Anim.SetFloat("Speed", Mathf.Abs(move));

        rig.velocity = new Vector2(move * maxSpeed * dashSpeed, rig.velocity.y);

        if (!grounded && dashTimer != 0 && airDash)
        {
            airDash = true;
            rig.gravityScale = 0;
            rig.velocity = new Vector2(move * maxSpeed * dashSpeed, 0);
        }

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        if (dashTimer > dashDuration)
        {
            if (airDash)
            {
                dashSpeed = 1;
                dashTimer = 0;

                airDash = false;
            }
            else if (grounded || water)
            {
                dashSpeed = 1;
                dashTimer = 0;
            }
        }

        if (dashSpeed == dashSpeedMultiplier)
        {
            dashTimer += Time.deltaTime;
        }
    }

    public void Jump(bool jump)
    {
        if ((grounded || joint.enabled || water) && jump) //&& m_Anim.GetBool("Ground"))
        {
            grounded = false;
            HookRelease();
            //m_Anim.SetBool("Ground", false);
            rig.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void HookShoot(Vector3 mousePos)
    {
        if (!joint.isActiveAndEnabled)
        {
            Vector3 hookPos = Camera.main.ScreenToWorldPoint(mousePos);
            hookPos.z = 0;

            float hookReach = 5f;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, hookPos - transform.position, hookReach, hookMask);

            if (hit.collider != null)
            {
                joint.enabled = true;
                joint.connectedAnchor = hit.point;
            }
        }
    }

    public void HookRelease()
    {
        joint.enabled = false;
    }

    public void Dash()
    {
        dashSpeed = dashSpeedMultiplier;
        if (!grounded)
        {
            airDash = true;
        }
    }

    public void Spike()
    {
        Die(); // TEMP
    }

    public void TakeDamage(bool instantDeath)
    {
        if (instantDeath)
        {
            Die();
        }
        else
        {
            //TAKE 1 HIT
            Debug.Log("Player Hit!!!!");
        }
    }

    public void Die()
    {
        joint.enabled = false;
        gameObject.transform.position = startPoint;
        //Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            water = true;
            jumpForceTemp = jumpForce;
            jumpForce *= waterMultiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            water = false;
            jumpForce = jumpForceTemp;
        }
    }
}