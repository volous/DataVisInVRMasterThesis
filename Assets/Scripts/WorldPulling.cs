using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private float _handDistance;
    private Vector3 _initialScale;
    private Vector3 _setRightPosition;
    private Vector3 _setLeftPosition;
    private Quaternion _setRightRotation;
    private Quaternion _setLeftRotation;

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
            //ScaleRig();
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
        _setRightRotation = xrRig.transform.localRotation * rightController.transform.rotation;
        if(_isRightTriggerDown && _isLeftTriggerDown) _handDistance = CalculateDistanceBetweenHands();
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
        _setLeftRotation = xrRig.transform.localRotation * leftController.transform.rotation;
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

    #region TranslationRotationAndScaleFunctions

    public void TranslateRig()
    {
        Vector3 newRightPos = _setRightPosition - rightController.transform.position;
        Vector3 newLeftPos = _setLeftPosition - leftController.transform.position;

        Vector3 newAvgPos = (newRightPos + newLeftPos) / 2;

        xrRig.transform.position = Vector3.Lerp(newAvgPos, xrRig.transform.position, translationScaler /1000);
        
    }

    public void RotateRig()
    {
        Quaternion _newRightRot = _setRightRotation * Quaternion.Inverse(rightController.transform.localRotation);
        Quaternion _newLeftRot = _setLeftRotation * Quaternion.Inverse(leftController.transform.localRotation);
        
        Quaternion _meanRot = Quaternion.Slerp(_newRightRot, _newLeftRot, .5f);

        xrRig.transform.localRotation = Quaternion.Lerp(_meanRot, xrRig.transform.localRotation, rotationScaler /1000);
    }

    public void ScaleRig()
    {
        float currentHandDistance = CalculateDistanceBetweenHands();
        float distanceDifference = currentHandDistance - _handDistance;

        if (distanceDifference is < 0.2f and > -0.2f) return;
        
        xrRig.transform.localScale = _initialScale + Vector3.one * -distanceDifference;
    }
    float CalculateDistanceBetweenHands()
    {
        return Vector3.Distance(rightController.transform.position, leftController.transform.position);
    }

    #endregion
    
}
