using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class WorldPulling : MonoBehaviour
{
    private bool _isRightTriggerDown, _isLeftTriggerDown;
    public GameObject xrRig, rightController, leftController, objectToRotate;
    
    public float translationScaler;
    public float rotationScaler;
    public float minScaleThreshold;
    
    public InputActionReference leftTriggerReference, rightTriggerReference;
  
    public float movSpeed = 5;
    
    private float _handDistance;
    private Vector3 _initialScale;
    private Vector3 _setRightPosition;
    private Vector3 _setLeftPosition;

    private Vector3 _previousLocation;
    
    private Quaternion _setRightRotation;
    private Quaternion _setLeftRotation;
    private Quaternion _setRotation, _offset;


    // Start is called before the first frame update
    void Start()
    {
        rightTriggerReference.action.performed += OnTriggerPressRight;
        leftTriggerReference.action.performed += OnTriggerPressLeft;

        rightTriggerReference.action.canceled += OnTriggerReleaseRight;
        leftTriggerReference.action.canceled += OnTriggerReleaseLeft;

        _initialScale = objectToRotate.transform.localScale;
    }

    private void Update()
    {
        if (_isRightTriggerDown && _isLeftTriggerDown)
        {
            TranslateRig();
            RotateRig();
            ScaleRig();
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
        _setRotation = Quaternion.Inverse(rightController.transform.rotation) * objectToRotate.transform.rotation;
        if(_isLeftTriggerDown) _handDistance = CalculateDistanceBetweenHands();


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
        _setRightPosition = xrRig.transform.position + rightController.transform.position;
        // _setLeftRotation = objectToRotate.transform.rotation * leftController.transform.rotation;
        
        Vector3 newRightPos = _setRightPosition - rightController.transform.position;
        Vector3 newLeftPos = _setLeftPosition - leftController.transform.position;
        Vector3 newAvgPos = (newRightPos + newLeftPos) / 2;
        _previousLocation = objectToRotate.transform.position - newAvgPos;

       
        
        if(_isRightTriggerDown) _handDistance = CalculateDistanceBetweenHands();
        
        
        
        // Quaternion _newRightRot = _setRightRotation * Quaternion.Inverse(rightController.transform.rotation);
        // Quaternion _newLeftRot = _setLeftRotation * Quaternion.Inverse(leftController.transform.rotation);
        // Quaternion _meanRot = Quaternion.Slerp(_newRightRot,_newLeftRot, .5f);
        // Debug.Log("trigMean"+_meanRot);
        // _previousRotation = objectToRotate.transform.rotation * _meanRot;  _setRotation = objectToRotate.transform.rotation;

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
        _initialScale = objectToRotate.transform.localScale;
        // _offset = objectToRotate.transform.rotation;
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

        objectToRotate.transform.position = _previousLocation - newAvgPos;
    }

    public void RotateRig()
    {
        Quaternion newRot = rightController.transform.rotation * _setRotation;
        objectToRotate.transform.rotation = Quaternion.Slerp(objectToRotate.transform.rotation, newRot, rotationScaler /1000);
        
        
        
        // Quaternion newRot = _offset * Quaternion.Inverse(rightController.transform.rotation);
        // objectToRotate.transform.rotation = newRot;
        // Quaternion _newRightRot = rightController.transform.rotation * Quaternion.Inverse(_setRightRotation);
        // Quaternion _newLeftRot = leftController.transform.rotation * Quaternion.Inverse(_setLeftRotation);
        // Quaternion _meanRot = Quaternion.Slerp(_newRightRot,_newLeftRot, .5f);
        // objectToRotate.transform.rotation = _previousRotation * Quaternion.Inverse(_meanRot);
        // Debug.Log("funcMean"+_meanRot);
    }

    public void ScaleRig()
    {
        float currentHandDistance = CalculateDistanceBetweenHands();
        float distanceDifference = currentHandDistance - _handDistance;
    
        if (distanceDifference is < 0.2f and > -0.2f) return;
        objectToRotate.transform.localScale = Vector3.Max(_initialScale + Vector3.one * distanceDifference, Vector3.one * 0.1f);
        
    }

    float CalculateDistanceBetweenHands()
    {
        return Vector3.Distance(rightController.transform.localPosition, leftController.transform.localPosition);
    }

    #endregion
    
}
