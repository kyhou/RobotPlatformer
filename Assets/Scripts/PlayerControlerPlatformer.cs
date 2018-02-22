using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlerPlatformer : PhysicsObject
{
    public float jumptakeOffSpeed = 7;
    public float maxSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    //private DistanceJoint2D joint;
    //[SerializeField]
    //private LayerMask hookMask;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //joint = GetComponent<DistanceJoint2D>();
        //animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        
        move.x = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumptakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            HookShoot(Input.mousePosition);
            joint.enabled = true;
        }*/

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityx", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}