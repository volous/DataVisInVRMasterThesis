using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;

public class GlyphSelection : MonoBehaviour
{
    [Header("Hand input action references")]
    public InputActionReference rightTriggerAction;
    public InputActionReference leftTriggerAction;
    
    [Header("Hand transforms")]
    public Transform rightHand;
    public Transform leftHand;

    // List of glyph positions
    private List<Vector3> glyphPositions = new List<Vector3>();

    private void Start()
    {
        // Subscribe to the action performed event
        rightTriggerAction.action.performed += OnPressed;
        //leftTriggerAction.action.performed += OnPressed;
    }

    private void OnDestroy()
    {
        // Unsubscribe to the action performed event
        rightTriggerAction.action.performed -= OnPressed;
        //leftTriggerAction.action.performed -= OnPressed;
    }

    private int ReturnGlyphNum()
    {
        // FindGlyph stores the positions of glyphs where either the left hand or right hand is located
        var findGlyph = glyphPositions.Where(locate => locate == leftHand.position || locate == rightHand.position);

        // Log a message indicating the glyph number
        Debug.Log($"Glyph num is: ");
        return 0;
    }
    
    // Method called when a glyph is pressed, takes the glyph number as input
    public void OnPressed(InputAction.CallbackContext context)
    {
        ReturnGlyphNum();
    }
    
}
