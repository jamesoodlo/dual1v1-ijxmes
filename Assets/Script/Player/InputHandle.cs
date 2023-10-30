using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputHandle : MonoBehaviour
{
    public bool attack, sprint, jump, block;
    public Vector2 move, look;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            attack = true;
        }
        if(context.performed)
        {
            attack = true;
        }
        if(context.canceled)
        {
            attack = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            block = true;
        }
        if(context.performed)
        {
            block = true;
        }
        if(context.canceled)
        {
            block = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            sprint = true;
        }
        if(context.performed)
        {
            sprint = true;
        }
        if(context.canceled)
        {
            sprint = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jump = true;
        }
        if(context.performed)
        {
            jump = true;
        }
        if(context.canceled)
        {
            jump = false;
        }
    }
}
