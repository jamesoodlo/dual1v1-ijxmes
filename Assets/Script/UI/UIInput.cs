using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    public bool escape;
    public bool tab;

    public void OnEscape(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            escape = true;
        }
        if(context.performed)
        {
            escape = true;
        }
        if(context.canceled)
        {
            escape = false;
        }
    }

     public void OnTab(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            tab = true;
        }
        if(context.performed)
        {
            tab = true;
        }
        if(context.canceled)
        {
            tab = false;
        }
    }
}
