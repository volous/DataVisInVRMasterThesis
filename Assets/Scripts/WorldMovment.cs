using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMovment : MonoBehaviour
{
    public GameObject xrRig;
    public Transform rightControllerTransform;

    [Header("Speed Controls")] 
    public float translationScaler;
    public float rotationScaler;
    
    private Vector3 _setPosition;
    private Quaternion _setRotation;

    private Vector3 _translationDir;
    private Quaternion _rotationalDir;

    private bool _isRightTriggerDown;
    
    [Header("Input actions")]
    public InputActionReference triggerRight;
    
    // Start is called before the first frame update
    void Start()
    {
        triggerRight.action.performed += RightTriggerPressed;
        triggerRight.action.canceled += RightTriggerLetGo;
        
        _translationDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Translate();
    }

    void Translate()
    {
        if (!_isRightTriggerDown) return;

        Vector3 newPos = _setPosition - rightControllerTransform.position;

        xrRig.transform.position = Vector3.Lerp(newPos, xrRig.transform.position, translationScaler);
    }

    void RightTriggerPressed(InputAction.CallbackContext context)
    {
        _setPosition = xrRig.transform.position + rightControllerTransform.position;
        _isRightTriggerDown = true;
    }
    
    void RightTriggerLetGo(InputAction.CallbackContext context)
    {
        _isRightTriggerDown = false;
    }
}
