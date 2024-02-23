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

    public HeaderDispUI headerDispUI;
    
    [Header("Dimentions")] 
    public string posX;
    public string posY;
    public string posZ;
    public string scale;
    public string colR;
    public string colG;
    public string colB;

    private string[,] _dataArray;
    private string[] _headers;
    private bool _isRunning;
    private Vector3[] _position;
    private float[] _scales;
    private Mesh[] _meshes;
    private Color[] _colors;
    private Material[] _materials;


    private void Start()
    {
        _isRunning = false;
    }

    public void ReceiveFeatures(string[] features)
    {
        posX = features[0];
        posY = features[1];
        posZ = features[2];
        scale = features[3];
        colR = features[4];
        colG = features[5];
        colB = features[6];
    }

    public void ReciveDataMatrix(string[,] dataArray, string[] headers)
    {
        _isRunning = true;
        
        _dataArray = dataArray;
        _headers = headers;

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

    private void BeginRendering()
    {
        headerDispUI.ReceiveFeatures(_headers);

        int nRows = _dataArray.GetLength(0);
        int nFeatures = _dataArray.GetLength(1);

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
            _scales[row] = float.Parse(_dataArray[row, FeatureBasedOnHeader(scale)]);
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
            _colors[row] = new Color(
                float.Parse(_dataArray[row, FeatureBasedOnHeader(colR)]) * 2,
                float.Parse(_dataArray[row, FeatureBasedOnHeader(colG)]) * 2,
                float.Parse(_dataArray[row, FeatureBasedOnHeader(colB)]) * 2,
                transparency
            );
        }

        // Materials
        for (int row = 0; row < nRows; row++)
        {
            Material nMat = new Material(_material);
            nMat.color = _colors[row];
            _materials[row] = nMat;
        }
    }

    private void Update()
    {
        if (!_isRunning) return;
        for (int i = 0; i < _position.Length; i++)
        {
            // Draw the mesh with the specified scale
            Graphics.DrawMesh(_meshes[i], _position[i], quaternion.identity, _materials[i], 0, null, 0, null, false,
                false, false);
        }
    }
}