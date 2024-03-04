using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    
    public InputActionReference triggerDown;
    public float movSpeed = 1;
    [SerializeField] private GameObject _controllerObject;
    [SerializeField] private Transform _facePos;

    private bool _isMoving = false;
    
    private void Start()
    {
        triggerDown.action.performed += OnTriggerPress;
        triggerDown.action.canceled += OnTriggerRelease;//Change
    }

    private void Update()
    {
        if (_isMoving)
        {
            Vector3 _direction = (_controllerObject.transform.position - _facePos.position).normalized;
            transform.Translate(_direction * Time.deltaTime * movSpeed);
            

            // Vector3 _direction = (_controllerObject.transform.localPosition - _facePos.position);
            // Vector3 _newPosition = _direction + transform.position;
            // //transform.Translate(_direction  * movSpeed * Time.deltaTime);
            // transform.position += _direction * movSpeed * Time.deltaTime;
            // // Debug.Log("controller" + _controllerObject.transform.position);
            // // Debug.Log("rig" + transform.position);
        }
        
    }

    private void OnDestroy()
    {
        triggerDown.action.performed -= OnTriggerPress;
        triggerDown.action.canceled -= OnTriggerRelease;//change
    }

    // Function to be triggered
    private void TriggerFunction()
    {
        // Your code here
        //Debug.Log("Trigger pressed!");
        _isMoving = true;
        
        
    }
    // Input action callback
    public void OnTriggerPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerFunction();
        }
    }
    private void ReleaseFunction()
    {
        //Debug.Log("Trigger Released!");
        _isMoving = false;

    }
    public void OnTriggerRelease(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            ReleaseFunction();
        }
    }
}
