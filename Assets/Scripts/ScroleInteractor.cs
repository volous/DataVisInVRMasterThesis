using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScroleInteractor : MonoBehaviour
{

    public Transform uiHandle;
    public Scrollbar scrollbar;

    public Transform rightHandTransform;
    public Transform leftHandTransform;
    
    // controler inputs
    public InputActionReference triggerRight;
    public InputActionReference triggerLeft;

    private bool _holdingHandleLeft;
    private bool _holdingHandleRight;

    private float _rightHandInitialYValue;
    private float _leftHandInitialYValue;

    // Start is called before the first frame update
    void Start()
    {
        triggerRight.action.performed += GrabHandle;
        triggerLeft.action.performed += GrabHandle;
        
        triggerRight.action.canceled += LetGoOfHandle;
        triggerLeft.action.canceled += LetGoOfHandle;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSlider();
    }

    void LetGoOfHandle(InputAction.CallbackContext context)
    {
        _holdingHandleLeft = false;
        _holdingHandleRight = false;
        transform.localPosition = uiHandle.transform.position;
        foreach (Transform child in transform)
        {
            child.parent = null;
        }

       
    }
    
    void GrabHandle(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.device.name);

        if (context.control.device.name == "OculusTouchControllerOpenXR1")
        {
            _holdingHandleRight = true;
            _rightHandInitialYValue = rightHandTransform.position.y - scrollbar.value;
        }
        else if (context.control.device.name == "OculusTouchControllerOpenXR")
        {
            _holdingHandleLeft = true;
            _leftHandInitialYValue = leftHandTransform.position.y - scrollbar.value;
        }
    }

    void MoveSlider()
    {
        try
        {
            if (_holdingHandleRight && transform.GetChild(0).name == "[Right Controller] Dynamic Attach")
            {
                scrollbar.value = -(_rightHandInitialYValue - rightHandTransform.position.y) ;
            }
            else if (_holdingHandleLeft && transform.GetChild(0).name == "[Left Controller] Dynamic Attach")
            {
                scrollbar.value = -(_leftHandInitialYValue - leftHandTransform.position.y);
            }
        }
        catch (Exception e)
        {
            
        }
    }
    
    
}
