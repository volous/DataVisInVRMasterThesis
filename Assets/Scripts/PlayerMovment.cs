using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    //different variables:
    //InputActionReference allows us to refer to the generated input map
    public InputActionReference triggerDown;
    //starting movement speed
    public float movSpeed = 1;
    //reference to the controller object
    [SerializeField] private GameObject _controllerObject; 
    //reference to the reference object
    [SerializeField] private GameObject _selectedReference;
    //bool to determine if the user is moving
    private bool _isMoving = false;
    
    private void Start()
    {
        //subscriptions to the function handling what happens on press and release
        triggerDown.action.performed += OnTriggerPress;
        triggerDown.action.canceled += OnTriggerRelease;
    }

    private void Update()
    {
        //if the moving boolean is true
        if (_isMoving)
        {
            //get the directinal difference between the controller and the reference object
            Vector3 _direction = (_controllerObject.transform.position - _selectedReference.transform.position);
            //translation using the direciton
            transform.Translate(_direction * Time.deltaTime * movSpeed);
        }
        
    }

    private void OnDestroy()
    {
        //unsubscribe to the press and release functions
        triggerDown.action.performed -= OnTriggerPress;
        triggerDown.action.canceled -= OnTriggerRelease;//change
    }

    private void TriggerFunction()
    {
        //sets bool to true
        _isMoving = true;
        //sets the position of the reference object to be where the controller was, when the input was made
        _selectedReference.transform.position = _controllerObject.transform.position;
        //activates the reference object, making it visible in the system
        _selectedReference.SetActive(true);

    }
    
    private void ReleaseFunction()
    {
        //sets bool to false
        _isMoving = false;
        //deactivates the reference object
        _selectedReference.SetActive(false);

    }
    // Input action callback
    public void OnTriggerPress(InputAction.CallbackContext context)
    {
        //runs the trigger function if the context has been subscribed to
        if (context.performed)
        {
            TriggerFunction();
        }
    }
    
    public void OnTriggerRelease(InputAction.CallbackContext context)
    {
        //runs the release function if the context has been subscribed to
        if (context.canceled)
        {
            ReleaseFunction();
        }
    }
}
