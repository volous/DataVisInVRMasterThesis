using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiMovment : MonoBehaviour
{
    public GameObject fullUIObject;
    public int speed;
    
    [Header("Input actions")]
    public InputActionReference xButtonDownLeft;
    public InputActionReference yButtonDownLeft;
    public InputActionReference thumbStickLeft;

    private Vector3 _directionVector;

    private void Start()
    {
        xButtonDownLeft.action.performed += TranslateZTowards;
        xButtonDownLeft.action.canceled += ZeroMovment;
        
        yButtonDownLeft.action.performed += TranslateZAway;
        yButtonDownLeft.action.canceled += ZeroMovment;
        
        thumbStickLeft.action.performed += ThumbStickInputs;
        thumbStickLeft.action.canceled += ZeroMovment;
        
        _directionVector = Vector3.zero;
    }

    void TranslateZAway(InputAction.CallbackContext context)
    {
        _directionVector = new Vector3(_directionVector.x, _directionVector.y, 1);
    }
    
    void TranslateZTowards(InputAction.CallbackContext context)
    {
        _directionVector = new Vector3(_directionVector.x, _directionVector.y, -1);
    }

    void ThumbStickInputs(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _directionVector = new Vector3(value.x, value.y, _directionVector.z);
    }

    void ZeroMovment(InputAction.CallbackContext context)
    {
        _directionVector = Vector3.zero;
    }

    private void Update()
    {
        Translate();
    }

    void Translate()
    {
        fullUIObject.transform.Translate(_directionVector * (speed * Time.deltaTime));
    }
}
