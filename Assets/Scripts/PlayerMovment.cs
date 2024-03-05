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
    [SerializeField] private GameObject _selectedReference;
    

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
            Vector3 _direction = (_controllerObject.transform.position - _selectedReference.transform.position);
            transform.Translate(_direction * Time.deltaTime * movSpeed);
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
        _selectedReference.transform.position = _controllerObject.transform.position;
        _selectedReference.SetActive(true);

    }
    
    private void ReleaseFunction()
    {
        //Debug.Log("Trigger Released!");
        _isMoving = false;
        _selectedReference.SetActive(false);

    }
    // Input action callback
    public void OnTriggerPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerFunction();
        }
    }
    
    public void OnTriggerRelease(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            ReleaseFunction();
        }
    }
}
