using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SocketClass : MonoBehaviour
{

    // for manipulator socket only
    public Slider slider;
    public DataPointsRenderer DPR;
    
    private GameObject _featureObject;
    public bool isManipulationSocket;
    public DimentionSelectionHandeler _dimentionSelectionHandeler;
    public int ID;
    // Declare a UnityEvent
    public UnityEvent functionToCall;

    [ContextMenu("CallStored")]
    // Method to call the stored function
    public void CallStoredFunction()
    {
        functionToCall.Invoke(); // Invoke the UnityEvent
    }

    private void Start()
    {
        //_dimentionSelectionHandeler = GameObject.Find("DimentionSelectionHandeler").GetComponent<DimentionSelectionHandeler>();
    }

    public GameObject GetFeatureObject()
    {
        return _featureObject;
    }

    public void AssignFeatureObject(GameObject featureObject)
    {
        _featureObject = featureObject;
    }

    public void RemoveFeatureObject()
    {

        _dimentionSelectionHandeler.AssignChoice(null, ID);
        _featureObject = null;
    }

    public void SetSliderValues(FeatureObject featureObject)
    {
        DPR.ResetFeatureFromName(featureObject.name);
        //slider.SetSliderValues(new Vector2(range.x, range.y));
    }

    private void Update()
    {
        // if (_featureObject == null)
        // {
        //     
        // }
    }
}
