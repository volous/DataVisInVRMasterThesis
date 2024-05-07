using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.VFX;


public class GlyphGrapping : MonoBehaviour
{
    public float publicSphereOfInfluenceRadius;
    private float _sphereOfInfluenceRadius;
    public DataPointsRenderer DPR;
    public VisualEffect pointCloudRenderer;
    
    [Space][Header("Prefab Shperes")] 
    public GameObject glyphHighlightSphere;
    public GameObject glyphSelectedSphere;
    
    [Space][Header("Hands")] 
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    private List<Vector3> _vector3List;
    private bool _isGlypsActive;

    // controler inputs
    public InputActionReference triggerRight;
    public InputActionReference triggerLeft;

    private GameObject _glyphHighlightLatestSphere;
    private GameObject _glyphSelectedLatestSphere;
    private float middleOfRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _isGlypsActive = false;
        middleOfRenderer = DPR.RederingArea / 2;
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
            Destroy(_glyphHighlightLatestSphere);
            Transform pcrTransform = pointCloudRenderer.transform;
            _sphereOfInfluenceRadius = DPR.GetInstanceFromPosition(glyphsInHand[0]).Scale;
            Vector3 glyphPos = GetGlobalChildPosition(pcrTransform.position, glyphsInHand[0], pcrTransform.rotation, pcrTransform.localScale, middleOfRenderer);
            GameObject highlightSphere = Instantiate(glyphHighlightSphere, glyphPos, pointCloudRenderer.transform.rotation);
            highlightSphere.transform.localScale *= _sphereOfInfluenceRadius+0.06f;
            _glyphHighlightLatestSphere = highlightSphere;
        }
        catch (Exception e)
        {
           
        }
        
        
        
    }
    
    // Function to calculate the global position of the child based on parent's position, child's local position, parent's rotation, and parent's scale
    public static Vector3 GetGlobalChildPosition(Vector3 parentPosition, Vector3 localChildPosition, Quaternion parentRotation, Vector3 parentScale, float middleOfsett)
    {
        Vector3 childPosWithOffset = new Vector3(
            localChildPosition.x - middleOfsett,
            localChildPosition.y - middleOfsett,
            localChildPosition.z - middleOfsett
            );
        
        // Apply the parent's scale to the local position of the child
        Vector3 scaledOffset = Vector3.Scale(childPosWithOffset, parentScale);

        // Rotate the scaled local position of the child based on the parent's rotation
        Vector3 rotatedOffset = parentRotation * scaledOffset;

        // Calculate the global position of the child by adding the rotated offset to the parent's position
        Vector3 globalChildPosition = parentPosition + rotatedOffset;

        return globalChildPosition;
    }

    List<Vector3> CheckMatches()
    {
        if (!_isGlypsActive) return new List<Vector3>();
        Transform pcrTransform = pointCloudRenderer.transform;
        //Debug.Log(_vector3List[0]+ pointCloudRenderer.transform.position +  " -- " + rightHandTransform.position);
        return _vector3List
            .Where(v => Vector3.Distance(GetGlobalChildPosition(pcrTransform.position, v, pcrTransform.rotation,pcrTransform.localScale, middleOfRenderer), rightHandTransform.position) <= publicSphereOfInfluenceRadius)
            .ToList();
    }

    void GrabGlyph(InputAction.CallbackContext context)
    {
        Vector3 handPos = rightHandTransform.position;
        List<Vector3> glyphsInHand = CheckMatches();
        if (glyphsInHand.Count > 0)
        {
            Debug.Log($"{glyphsInHand[0]} is withing {_sphereOfInfluenceRadius} distance from {handPos}");
            
            Destroy(_glyphSelectedLatestSphere);
            Transform pcrTransform = pointCloudRenderer.transform;
            Vector3 glyphPos = GetGlobalChildPosition(pcrTransform.position, glyphsInHand[0], pcrTransform.rotation, pcrTransform.localScale, middleOfRenderer);
            GameObject selectedSphere = Instantiate(glyphSelectedSphere, glyphPos, pointCloudRenderer.transform.rotation);
            selectedSphere.transform.localScale *= _sphereOfInfluenceRadius +0.06f;
            selectedSphere.transform.SetParent(pointCloudRenderer.gameObject.transform);
            _glyphSelectedLatestSphere = selectedSphere;
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
        
    }

    public void SetGlyphsActive(bool state)
    {
        _isGlypsActive = state;
    }
}