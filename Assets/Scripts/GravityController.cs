using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private bool water;
    private float fallMultiplier = 5f;
    private float lowJumpMultiplier = 4f;

    private Rigidbody2D rig;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!water)
        {
            if (rig.velocity.y < 0)
            {
                rig.gravityScale = fallMultiplier;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            water = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            water = false;
        }
    }
}