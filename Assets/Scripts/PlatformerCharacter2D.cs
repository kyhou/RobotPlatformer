using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)]
    [SerializeField]
    private float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField]
    private bool airControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask whatIsGround;                  // A mask determining what is ground to the character

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    private Transform ceilingCheck;   // A position marking where to check for ceilings
    const float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    //private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D rig;
    private bool facingRight = true;  // For determining which way the player is currently facing.

    private DistanceJoint2D joint;
    private float fallMultiplier = 3f;
    private float lowJumpMultiplier = 2f;
    private float dashSpeed = 10f;
    [SerializeField]
    private LayerMask hookMask;
    private Transform rightCheck;
    private Transform leftCheck;

    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        rightCheck = transform.Find("RightCheck");
        leftCheck = transform.Find("LeftCheck");
        //m_Anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();

        joint.enabled = false;
        joint.distance = GetComponent<BoxCollider2D>().size.x / 2;
    }


    private void FixedUpdate()
    {
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }
        //m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

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
            rig.gravityScale = 1f;
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch )//&& m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround) && crouch)
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        //m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * crouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            //m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            rig.velocity = new Vector2(move * maxSpeed, rig.velocity.y);
            
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if ((grounded || joint.enabled) && jump) //&& m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            grounded = false;
            HookRelease();
            //m_Anim.SetBool("Ground", false);
            rig.AddForce(new Vector2(0f, jumpForce)); //, ForceMode2D.Impulse);
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
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
        rig.AddForce(new Vector2(dashSpeed, 0f),ForceMode2D.Impulse);
    }

    public void Spike()
    {
        Destroy(this.gameObject);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coins")
        {

            collision.GetComponent<Coins>().collectCoin(collision.transform.position, facingRight);
        }
    }*/
}