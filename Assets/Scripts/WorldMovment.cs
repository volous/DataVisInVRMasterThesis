using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMovment : MonoBehaviour
{
    public bool worldPull;
    public GameObject xrRig;
    public Transform rightControllerTransform;
    public Transform leftControllerTransform;
    public UiMovment uiMovment;
    
    [Header("Speed Controls")] 
    public float translationScaler;
    public float rotationScaler;
    
    private Vector3 _setPosition;
    private Quaternion _setRotation;
    private float _handDistance;
    private Vector3 _initialScale;

    private Vector3 _setRightPosition;
    private Vector3 _setLeftPosition;
    
    
    private Vector3 _translationDir;
    private Quaternion _rotationalDir;

    private bool _isRightTriggerDown;
    private bool _isLeftTriggerDown;

    [Header("Input actions")]
    public InputActionReference triggerRight;
    public InputActionReference triggerLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        triggerRight.action.performed += RightTriggerPressed;
        triggerRight.action.canceled += RightTriggerLetGo;
        
        triggerLeft.action.performed += LeftTriggerPressed;
        triggerLeft.action.canceled += LeftTriggerLetGo;
        
        _translationDir = Vector3.zero;
        _initialScale = xrRig.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (worldPull)
        {
            if (!_isRightTriggerDown) return;
            //Translate();
            // Rotate();
        
            if (!_isLeftTriggerDown) return;
            //Scale(); 
        }
        
    }

    void Translate()
    {


        Vector3 newPos = _setPosition - rightControllerTransform.position;
        xrRig.transform.position = Vector3.Lerp(newPos, xrRig.transform.position, translationScaler/1000);
    }

    void Rotate()
    {

        Quaternion newRot = _setRotation * Quaternion.Inverse(rightControllerTransform.rotation);
        xrRig.transform.rotation = Quaternion.Lerp(newRot, xrRig.transform.rotation, rotationScaler /1000);
    }

    void Scale()
    {
        float currentHandDistance = CalculateDistanceBetweenHands();
        float distanceDifference = currentHandDistance - _handDistance;

        if (distanceDifference is < 0.2f and > -0.2f) return;
        
        xrRig.transform.localScale = _initialScale + Vector3.one * -distanceDifference;
    }

    void RightTriggerPressed(InputAction.CallbackContext context)
    {
        _setPosition = xrRig.transform.position + rightControllerTransform.position;
        _setRotation = xrRig.transform.rotation * rightControllerTransform.rotation;
        _isRightTriggerDown = true;
        
        if (_isLeftTriggerDown)
        {
            _handDistance = CalculateDistanceBetweenHands();
        }
    }
    
    void RightTriggerLetGo(InputAction.CallbackContext context)
    {
        _isRightTriggerDown = false;
        _initialScale = xrRig.transform.localScale;
        uiMovment.speed = (int)xrRig.transform.localScale.x;
    }

    void LeftTriggerPressed(InputAction.CallbackContext context)
    {
        _isLeftTriggerDown = true;

        if (_isRightTriggerDown)
        {
            _handDistance = CalculateDistanceBetweenHands();
        }
    }
    
    void LeftTriggerLetGo(InputAction.CallbackContext context)
    {
        _isLeftTriggerDown = false;
        _initialScale = xrRig.transform.localScale;
        uiMovment.speed = (int)xrRig.transform.localScale.x;
    }

    float CalculateDistanceBetweenHands()
    {
        return Vector3.Distance(rightControllerTransform.position, leftControllerTransform.position);
    }
}
