using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DimentionObject : MonoBehaviour
{
    public TMP_Text DimentionText;
    public int ID;
    public Transform rightHandTransform;
    private FeatureObject _featureObject;


    public void AssignFeature(FeatureObject featureObject)
    {
        if (_featureObject != null)
        {
            _featureObject.UnChoose();
        }
        
        _featureObject = featureObject;
        DimentionText.text = featureObject.GetFeature();
        //gameObject.GetComponent<XRSimpleInteractable>().enabled = true;
    }

    public void GrabInteraction()
    {
        if (_featureObject == null) return;
        
        foreach (Renderer childRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            childRenderer.material.color = Color.red;
        }
        
        _featureObject.UnChoose();
        DimentionText.text = "Feature";
        _featureObject.UnAsignDimention(ID);
        _featureObject = null;
    }
    
    public void HoverInteraction()
    {
        if (_featureObject == null) return;
        
        // _featureObject.gameObject.GetComponent<Collider>().isTrigger = false;
        // _featureObject.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
        
        _featureObject.transform.position = rightHandTransform.position;
    }
    
    public void ExitHoverInteraction()
    {
        if (_featureObject == null) return;
        
        
        _featureObject.ReturnToBoardPosition();
    }
}
