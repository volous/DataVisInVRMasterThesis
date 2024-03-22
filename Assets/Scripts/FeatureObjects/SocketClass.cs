using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SocketClass : MonoBehaviour
{

    private GameObject _featureObject;
    
    // Declare a UnityEvent
    public UnityEvent functionToCall;

    [ContextMenu("CallStored")]
    // Method to call the stored function
    public void CallStoredFunction()
    {
        functionToCall.Invoke(); // Invoke the UnityEvent
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
        _featureObject = null;
    }
}
