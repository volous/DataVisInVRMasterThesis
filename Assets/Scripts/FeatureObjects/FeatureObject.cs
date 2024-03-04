using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureObject : MonoBehaviour
{
    public string feature;
    public Vector3 boardPosition;
    public FeatureObjectsHandeler featureObjectsHandeler;
    public DimentionSelectionHandeler dimentionSelectionHandeler;

    private GameObject _currentCollisionObject;


    private void Start()
    {
        dimentionSelectionHandeler = GameObject.Find("DimentionSelectionHandeler").GetComponent<DimentionSelectionHandeler>();
    }

    public void AssignedToDimention()
    {
        
    }

    public void LetGoOfObject()
    {
        transform.position = boardPosition;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        if (_currentCollisionObject != null)
        {
            dimentionSelectionHandeler.AssignChoice(feature, int.Parse(_currentCollisionObject.name));
            _currentCollisionObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DimentionCollider"))
        {
            Color color = other.GetComponent<Renderer>().material.color;
            color.a = 0.5f;
            other.GetComponent<Renderer>().material.color = color;

            _currentCollisionObject = other.gameObject;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DimentionCollider"))
        {
            Color color = other.GetComponent<Renderer>().material.color;
            color.a = 1;
            other.GetComponent<Renderer>().material.color = color;

            _currentCollisionObject = null;
        }
    }
}
