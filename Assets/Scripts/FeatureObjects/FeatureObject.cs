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
    private MeshRenderer _meshRenderer;
    private Material _startingMaterial;

    private GameObject _socketObject;
    private GameObject _lastSocket; // the last socket that this object was assiged too
    private GameObject _boardObject;

    private Color _baseColor;

    private FeatureManipulation _featureManipulation;

    public Vector2 featureRange;
    


    private void Start()
    {
        featureRange = new Vector2(0,1);
        _boardObject = transform.parent.gameObject;
        boardPosition = gameObject.transform.localPosition;
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _startingMaterial = _meshRenderer.material;
        dimentionSelectionHandeler = GameObject.Find("DimentionSelectionHandeler").GetComponent<DimentionSelectionHandeler>();
        _featureManipulation = GameObject.Find("ManibulationHandeler").GetComponent<FeatureManipulation>();
    }

    public void UnChoose()
    {
        _meshRenderer.material = _startingMaterial;
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
        if (_socketObject != null)
        {
            _socketObject.GetComponent<SocketClass>().RemoveFeatureObject();
        }
    }

    public void CallAsignFeature()
    {
        if (_socketObject == null) return;

        SocketClass socketClass = _socketObject.GetComponent<SocketClass>();
        dimentionSelectionHandeler.AssignChoice(feature, socketClass.ID);
    }

    public void LetGoOfObject(bool isInSocket = false)
    {
        if (_socketObject != null && !isInSocket)
        {
            transform.SetParent(_socketObject.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = _socketObject.transform.localRotation;
            transform.localScale = Vector3.one;

            SocketClass socketClass = _socketObject.GetComponent<SocketClass>();
            _lastSocket = _socketObject;

            GameObject objectCurrentlyInSocket = socketClass.GetFeatureObject();
            if (objectCurrentlyInSocket != null)
            {
                objectCurrentlyInSocket.GetComponent<FeatureObject>().LetGoOfObject(true);
            }
            
            socketClass.AssignFeatureObject(gameObject);
            if (!socketClass.isManipulationSocket)
            {
                CallAsignFeature();
            }
            else
            {
                _featureManipulation.DataFromDPR(feature);
            }
            
            return;
        }

        if (_lastSocket != null)
        {
            SocketClass socketClass = _lastSocket.GetComponent<SocketClass>();
            if (socketClass.isManipulationSocket)
            {
                _featureManipulation.ManipulateData(feature, featureRange); // the range defined
            }
            
            socketClass.AssignFeatureObject(null);
        }
        
        transform.SetParent(_boardObject.transform);
        gameObject.GetComponent<Collider>().isTrigger = false;
        
        transform.localPosition = boardPosition;
        transform.localScale = boardScaler;
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        
    }
    

    private void Update()
    {
    }

    public void SetText(string text)
    {
        featureTextFront.text = text;
        featureTextTop.text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Socket"))
        {
            _socketObject = other.gameObject;
            _baseColor =_socketObject.GetComponent<Renderer>().material.color;
            _socketObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Socket"))
        {
            _socketObject.GetComponent<Renderer>().material.color = _baseColor;
            _socketObject = null;
        }
    }
}
