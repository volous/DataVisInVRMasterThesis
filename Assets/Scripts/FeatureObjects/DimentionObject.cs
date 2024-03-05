using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DimentionObject : MonoBehaviour
{
    public TMP_Text DimentionText;
    public int ID;
    
    private FeatureObject _featureObject;

    public void AssignFeature(FeatureObject featureObject)
    {
        if (_featureObject != null)
        {
            _featureObject.UnChoose();
        }
        
        _featureObject = featureObject;
        DimentionText.text = featureObject.GetFeature();
        gameObject.GetComponent<XRSimpleInteractable>().enabled = true;
        gameObject.GetComponent<Collider>().isTrigger = false;
    }

    public void GrabInteraction()
    {
        Debug.Log(_featureObject.name);
        gameObject.GetComponent<XRSimpleInteractable>().enabled = false;
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
