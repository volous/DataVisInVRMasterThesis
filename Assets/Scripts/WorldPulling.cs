using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class WorldPulling : MonoBehaviour
{
    
    public float translationScaler;
    public float rotationScaler;

    
    public InputActionReference leftTriggerReference, rightTriggerReference;
    public GameObject xrRig, rightController, leftController;
    public float movSpeed = 5;
    
    private bool _isRightTriggerDown, _isLeftTriggerDown;
    
    private Vector3 _setRightPosition;
    private Vector3 _setLeftPosition;
    private Quaternion _setRotation;
    
    [SerializeField] private Transform _worldControllerReferenceRight, _worldControllerReferenceLeft, _medianTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        rightTriggerReference.action.performed += OnTriggerPressRight;
        leftTriggerReference.action.performed += OnTriggerPressLeft;

        rightTriggerReference.action.canceled += OnTriggerReleaseRight;
        leftTriggerReference.action.canceled += OnTriggerReleaseLeft;
        
    }

    private void Update()
    {
        if (_isRightTriggerDown && _isLeftTriggerDown)
        {
            TranslateRig();
            RotateRig();
        }
    }

    #region TriggerAndReleaseHandling

    private void OnDestroy()
    {
        rightTriggerReference.action.performed -= OnTriggerPressRight;
        leftTriggerReference.action.performed -= OnTriggerPressLeft;

        rightTriggerReference.action.canceled -= OnTriggerReleaseRight;
        leftTriggerReference.action.canceled -= OnTriggerReleaseLeft;
    }

    public void OnTriggerPressRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RightTriggerFunction();
        }
    }

    public void RightTriggerFunction()
    {
        _isRightTriggerDown = true;
        _setRightPosition = xrRig.transform.position + rightController.transform.position;
        _setRotation = xrRig.transform.rotation * rightController.transform.rotation;
    }
    
    public void OnTriggerPressLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftTriggerFunction();
        }
    }
    
    public void LeftTriggerFunction()
    {
        _isLeftTriggerDown = true;
        _setLeftPosition = xrRig.transform.position + leftController.transform.position;
    }
    
    public void OnTriggerReleaseRight(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            RightReleaseFunction();
        }
    }

    public void RightReleaseFunction()
    {
        _isRightTriggerDown = false;
    }
    
    public void OnTriggerReleaseLeft(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            LeftReleaseFunction();
        }
    }
    
    public void LeftReleaseFunction()
    {
        _isLeftTriggerDown = false;
    }
    
    #endregion

    public void TranslateRig()
    {
        Vector3 newRightPos = _setRightPosition - rightController.transform.position;
        Vector3 newLeftPos = _setLeftPosition - leftController.transform.position;

        Vector3 newAvgPos = (newRightPos + newLeftPos) / 2;

        xrRig.transform.position = Vector3.Lerp(newAvgPos, xrRig.transform.position, translationScaler /1000);
        
    }

    public void RotateRig()
    {
        Quaternion newRot = _setRotation * Quaternion.Inverse(rightController.transform.rotation);
        xrRig.transform.rotation = Quaternion.Lerp(newRot, xrRig.transform.rotation, rotationScaler /1000);
    }

    public void ScaleRig()
    {
        
    }
}
