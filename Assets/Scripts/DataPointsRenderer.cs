using System;
using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Random = UnityEngine.Random;

public class DataPointsRenderer : MonoBehaviour
{
    public int RederingArea;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [Range(0,1)] public float transparency;

    [Header("Partical System")] 
    public ParticalRenderer particalRenderer;
    
    public float size = 1;
    public FeatureObjectsHandeler featureObjectsHandeler;
    
    [Header("Dimentions")] 
    public string posX;
    public string posY;
    public string posZ;
    public string scale;
    public string col;

    private string[,] _dataArray;
    private string[] _headers;
    private bool _isRunning;
    private Vector3[] _position;
    private float[] _scales;
    private Mesh[] _meshes;
    private Color[] _colors;
    private Material[] _materials;

    private List<GameObject> _glyphsList;


    private void Start()
    {
        _isRunning = false;
    }

    public void SetIsRunning(bool state)
    {
        _isRunning = state;
    }

    public void ReceiveFeatures(string[] features)
    {
        posX = features[0];
        posY = features[1];
        posZ = features[2];
        scale = features[3];
        col = features[4];
    }

    public void ReciveDataMatrix(string[,] dataArray, string[] headers)
    {
        _dataArray = dataArray;
        _headers = headers;
        
        featureObjectsHandeler.ReciveFeatureString(_headers);
        //BeginRendering();
    }

    public void ReciveDataMatrix( string[] headers)
    {
        ReceiveFeatures(headers);
        BeginRendering();
    }

    int FeatureBasedOnHeader(string header)
    {
        int r = -1;
        for (int i = 0; i < _headers.Length; i++)
        {
            if (_headers[i] == header)
            {
                r = i;
                break;
            }
        }

        return r; // this means that no header is matching. THis should never happen and it means there is an error
    }

    public void BeginRendering()
    {
        _isRunning = true;
 
        int nRows = _dataArray.GetLength(0) -1;
        int nFeatures = _dataArray.GetLength(1)-1;
        
        _position = new Vector3[nRows];
        _scales = new float[nRows];
        _meshes = new Mesh[nRows];
        _colors = new Color[nRows];
        _materials = new Material[nRows];

        // positions
        for (int row = 0; row < nRows; row++)
        {
            _position[row] = new Vector3(
                float.Parse(_dataArray[row, FeatureBasedOnHeader(posX)]) * RederingArea,
                float.Parse(_dataArray[row, FeatureBasedOnHeader(posY)]) * RederingArea,
                float.Parse(_dataArray[row, FeatureBasedOnHeader(posZ)]) * RederingArea
            );
        }

        //scales
        for (int row = 0; row < nRows; row++)
        {
            _scales[row] = (float.Parse(_dataArray[row, FeatureBasedOnHeader(scale)]) + 0.05f) * size;
        }

        //Meshes
        for (int row = 0; row < nRows; row++)
        {
            Mesh meshCopy = new Mesh();
            meshCopy.vertices = _mesh.vertices;
            meshCopy.triangles = _mesh.triangles;
            meshCopy.normals = _mesh.normals;
            meshCopy.uv = _mesh.uv;

            // Scale the vertices of the copied mesh
            Vector3[] scaledVertices = new Vector3[meshCopy.vertices.Length];
            for (int i = 0; i < meshCopy.vertices.Length; i++)
            {
                scaledVertices[i] = meshCopy.vertices[i] * _scales[row];
            }

            // Update the vertices of the copied mesh
            meshCopy.vertices = scaledVertices;


            _meshes[row] = meshCopy;
        }


        // Colors
        for (int row = 0; row < nRows; row++)
        {
            _colors[row] = RainbowColorFromFloat(float.Parse(_dataArray[row, FeatureBasedOnHeader(col)]));
        }

        // Materials
        for (int row = 0; row < nRows; row++)
        {
            Material nMat = new Material(_material);
            nMat.color = _colors[row];
            _materials[row] = nMat;
        }
        
        Debug.Log("set voxels");
        particalRenderer.SetVoxels(_position, _scales, _colors);
    }

    Color RainbowColorFromFloat(float value)
    {
        // Hue goes from 0 to 1, representing the entire color spectrum
        float hue = value / 2; // devided by 2 becous hue is a circle, so 0 and 1 would be the same color

        // Saturation and value are set to 1 for full color intensity
        float saturation = 1f;
        float valueIntensity = 1f;

        // Convert HSV to RGB
        Color rainbowColor = Color.HSVToRGB(hue, saturation, valueIntensity);
        return rainbowColor;
    }

    private void Update()
    {
        if (!_isRunning) return;
        return;
        for (int i = 0; i < _position.Length; i++)
        {
            // Draw the mesh with the specified scale
            Graphics.DrawMesh(_meshes[i], _position[i], quaternion.identity, _materials[i], 0, null, 0, null, false,
                false, false);
        }
    }
}