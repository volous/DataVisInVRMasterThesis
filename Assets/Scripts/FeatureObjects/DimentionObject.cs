using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DimentionObject : MonoBehaviour
{
    public TMP_Text DimentionText;
    
    private FeatureObject _featureObject;

    public void AssignFeature(FeatureObject featureObject)
    {
        if (_featureObject != null)
        {
            _featureObject.UnChoose();
        }
        
        _featureObject = featureObject;
        DimentionText.text = featureObject.GetFeature();
    }
}
