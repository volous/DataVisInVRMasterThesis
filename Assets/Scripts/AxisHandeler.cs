using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxisHandeler : MonoBehaviour
{
    public GameObject axiesObject;
    public GameObject boardObject;
    
    public InputActionReference xButtonDownRight;
    public InputActionReference bButtonDownRight;
    
    // Start is called before the first frame update
    void Start()
    {
        xButtonDownRight.action.performed += ShowAndHideAxies;
        bButtonDownRight.action.performed += ShowAndHideBoard;
    }
    

    void ShowAndHideAxies(InputAction.CallbackContext context)
    {
        axiesObject.gameObject.SetActive(!axiesObject.gameObject.activeSelf);
    }
    
    void ShowAndHideBoard(InputAction.CallbackContext context)
    {
        boardObject.gameObject.SetActive(!boardObject.gameObject.activeSelf);
    }
}
