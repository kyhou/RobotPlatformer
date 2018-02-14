using System;
using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D character;
    private bool jump, water;


    private void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
    }


    private void Update()
    {
        /*if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            jump = Input.GetButtonDown("Jump");
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            character.HookShoot(Input.mousePosition);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = Input.GetButtonDown("Jump");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            character.Dash();
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float h = Input.GetAxis("Horizontal");
        // Pass all parameters to the character control script.
        character.Move(h, crouch, jump, water);
        jump = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            water = true;
        }
        else
        {
            water = false;
        }
    }
}