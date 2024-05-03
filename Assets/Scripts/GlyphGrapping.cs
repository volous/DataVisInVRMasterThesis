using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.VFX;


public class GlyphGrapping : MonoBehaviour
{
    public float sphereOfInfluenceRadius;
    public Vector3 testHandPos;
    public GameObject testPrefab;
    public VisualEffect pointCloudRenderer;
    
    [Space][Header("Hands")] 
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    private List<Vector3> _vector3List;
    private bool _isGlypsActive;

    // controler inputs
    public InputActionReference triggerRight;
    public InputActionReference triggerLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        _isGlypsActive = false;

        triggerRight.action.performed += GrabGlyph;
    }

    private void Update()
    {
        if (!_isGlypsActive) return;
        
        HighlightGlyphsOnHover();
    }

    void HighlightGlyphsOnHover()
    {
        List<Vector3> glyphsInHand = CheckMatches();

        try
        {
            // instanciate for highlight, remember to destroy
            //Instantiate(testPrefab, glyphsInHand[0], testPrefab.transform.rotation);
        }
        catch (Exception e)
        {
           
        }
        
    }

    List<Vector3> CheckMatches()
    {
        if (!_isGlypsActive) return new List<Vector3>();

        return _vector3List
            .Where(v => Vector3.Distance(v, rightHandTransform.position) <= sphereOfInfluenceRadius)
            .ToList();
    }

    void GrabGlyph(InputAction.CallbackContext context)
    {
        Vector3 handPos = rightHandTransform.position;
        List<Vector3> glyphsInHand = CheckMatches();
        if (glyphsInHand.Count != 0)
        {
            Debug.Log($"{glyphsInHand[0]} is withing {sphereOfInfluenceRadius} distance from {handPos}");
        }
        else
        {
            Debug.Log($"no matches for hand pos {handPos}");
        }
    }

    public void ReciveVector3List(List<Vector3> vector3List)
    {
        _isGlypsActive = true;
        _vector3List = vector3List;

        foreach (Vector3 v in _vector3List)
        {
            Debug.Log(v + pointCloudRenderer.transform.position);
        }
        //_vector3List = pointCloudRenderer.GetVector3(0);
    }

    public void SetGlyphsActive(bool state)
    {
        _isGlypsActive = state;
    }
}