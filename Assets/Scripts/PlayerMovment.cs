using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    
    public InputActionReference triggerDown;
    
    
    // Function to be triggered
    private void TriggerFunction()
    {
        // Your code here
        Debug.Log("Trigger pressed!");
    }

    // Input action callback
    public void OnTriggerPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerFunction();
        }
    }
}
