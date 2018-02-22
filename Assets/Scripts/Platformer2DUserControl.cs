using System;
using UnityEngine;
using Gamelogic.Extensions;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D character;

    private float horizontalAxis;
    private bool dash = false;

    private ObservedValue<bool> jump = new ObservedValue<bool>(false);
    public event Action<bool> Grab = delegate { };

    private bool pickItem = false;

    private void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
    }

    private void Start()
    {
        jump.OnValueChange += Jump_OnValueChange;
    }

    private void Jump_OnValueChange()
    {
        if(jump.Value)
            character.Jump(jump.Value);
        jump.Value = false;
    }

    private void Update()
    {
        if (!jump.Value)
        {
            // Read the jump input in Update so button presses aren't missed.
            //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            jump.Value = Input.GetButtonDown("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            character.HookShoot(Input.mousePosition);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = Input.GetKeyDown(KeyCode.LeftShift);
        }

        pickItem = Input.GetKey(KeyCode.C);
        
        Grab(pickItem);        
    }


    private void FixedUpdate()
    {
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        horizontalAxis = Input.GetAxis("Horizontal");
        character.Move(horizontalAxis, crouch);

        if (dash)
        {
            character.Dash();
            dash = false;
        }
    }
}