using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxisHandeler : MonoBehaviour
{
    public GameObject axiesObject;
    
    public InputActionReference xButtonDownRight;
    // Start is called before the first frame update
    void Start()
    {
        xButtonDownRight.action.performed += ShowAndHideAxies;
    }
    

    void ShowAndHideAxies(InputAction.CallbackContext context)
    {
        axiesObject.gameObject.SetActive(!axiesObject.gameObject.activeSelf);
    }
}
