using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class FeatureObject : MonoBehaviour
{
    public string feature;
    public Vector3 boardPosition;
    public Vector3 boardScaler;
    public Material inUseMaterial;
    public FeatureObjectsHandeler featureObjectsHandeler;
    public DimentionSelectionHandeler dimentionSelectionHandeler;
    public TMP_Text featureTextFront;
    public TMP_Text featureTextTop;

    private GameObject _currentCollisionObject;
    private bool _isInUse;
    private MeshRenderer _meshRenderer;
    private Material _startingMaterial;


    private void Start()
    {
        boardPosition = gameObject.transform.localPosition;
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _startingMaterial = _meshRenderer.material;
        dimentionSelectionHandeler = GameObject.Find("DimentionSelectionHandeler").GetComponent<DimentionSelectionHandeler>();
    }

    public void AssignedToDimention()
    {
        
    }

    public void UnChoose()
    {
        _meshRenderer.material = _startingMaterial;
        _isInUse = false;
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;
    }

    public void ReturnToBoardPosition()
    {
        transform.localPosition = boardPosition;
    }

    public string GetFeature()
    {
        return feature;
    }

    public void UnAsignDimention(int ID)
    {
        dimentionSelectionHandeler.AssignChoice(null, ID);
    }

    public void Grabed()
    {
        gameObject.GetComponent<Collider>().isTrigger = true;
    }

    public void LetGoOfObject()
    {
        gameObject.GetComponent<Collider>().isTrigger = false;
        
        transform.localPosition = boardPosition;
        transform.localScale = boardScaler;
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        if (_currentCollisionObject != null)
        {
            _isInUse = true;
            gameObject.GetComponent<XRGrabInteractable>().enabled = false;

            DimentionObject collitionDimentionObject = _currentCollisionObject.GetComponent<DimentionObject>();
            
            dimentionSelectionHandeler.AssignChoice(feature, collitionDimentionObject.ID);
            foreach (Renderer childRenderer in _currentCollisionObject.GetComponentsInChildren<Renderer>())
            {
                childRenderer.material.color = Color.green;
            }
            collitionDimentionObject.AssignFeature(this);
        }
    }
    

    private void Update()
    {
        if (_isInUse)
        {
            _meshRenderer.material = inUseMaterial;
        }
    }

    public void SetText(string text)
    {
        featureTextFront.text = text;
        featureTextTop.text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DimentionCollider"))
        {
            Color color = other.GetComponentInChildren<Renderer>().material.color;
            color.a = 0.5f;
            foreach (Renderer childRenderer in other.GetComponentsInChildren<Renderer>())
            {
                childRenderer.material.color = color;
            }
            _currentCollisionObject = other.gameObject;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DimentionCollider"))
        {
            Color color = other.GetComponentInChildren<Renderer>().material.color;
            color.a = 1;
            foreach (Renderer childRenderer in other.GetComponentsInChildren<Renderer>())
            {
                childRenderer.material.color = color;
            }
            _currentCollisionObject = null;
        }
    }
}
