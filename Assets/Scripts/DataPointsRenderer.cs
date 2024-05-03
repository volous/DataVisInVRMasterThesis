using System;
using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Random = UnityEngine.Random;
using System.Diagnostics;

public class DataPointsRenderer : MonoBehaviour
{
    public int RederingArea;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [Range(0,1)] public float transparency;

    [Header("Partical System")] 
    public PointCloudRenderer pointCloudRenderer;
    
    public float size = 1;
    public FeatureObjectsHandeler featureObjectsHandeler;
    
    [Header("Dimentions")] 
    public string posX;
    public string posY;
    public string posZ;
    public string scale;
    public string col;

    private string[,] _originalDataArray;
    private string[,] _manipulatedDataArray;
    
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
        _originalDataArray = dataArray; // the array to be read by for original data values
        _manipulatedDataArray = dataArray; // the array to be used for displaying the data
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
        Stopwatch stopwatch = Stopwatch.StartNew();
        _isRunning = true;
        pointCloudRenderer._vfx.enabled = true;
        int nRows = _manipulatedDataArray.GetLength(0) - 1;

        // Initialize arrays
        _position = new Vector3[nRows];
        _scales = new float[nRows];
        _meshes = new Mesh[nRows];
        _colors = new Color[nRows];
        _materials = new Material[nRows];

        // positions
        for (int row = 1; row < nRows; row++)
        {
            _position[row] = new Vector3(
                float.Parse(_manipulatedDataArray[row, FeatureBasedOnHeader(posX)]) * RederingArea,
                float.Parse(_manipulatedDataArray[row, FeatureBasedOnHeader(posY)]) * RederingArea,
                float.Parse(_manipulatedDataArray[row, FeatureBasedOnHeader(posZ)]) * RederingArea
            );
        }

        //scales
        for (int row = 0; row < nRows; row++)
        {
            _scales[row] = (float.Parse(_manipulatedDataArray[row, FeatureBasedOnHeader(scale)]) + 0.05f) * size;
        }


        // Colors
        for (int row = 0; row < nRows; row++)
        {
            _colors[row] = RainbowColorFromFloat(float.Parse(_manipulatedDataArray[row, FeatureBasedOnHeader(col)]));
        }

        pointCloudRenderer.SetParticals(_position, _scales, _colors);
    }
    

    Color RainbowColorFromFloat(float value)
    {
        // Hue goes from 0 to 1, representing the entire color spectrum
        float hue = value / 2; // divided by 2 because hue is a circle, so 0 and 1 would be the same color

        // Saturation and value are set to 1 for full color intensity
        float saturation = 1f;
        float valueIntensity = 1f;

        // Convert HSV to RGB
        Color rainbowColor = Color.HSVToRGB(hue, saturation, valueIntensity);
        return rainbowColor;
    }

    public void StopRendering()
    {
        pointCloudRenderer._vfx.enabled = false;
    }

    int LoactionFromName(string name)
    {
        //first get the location int of the feature based on the name
        int location = 0;
        for (int i = 0; i < _headers.Length; i++)
        {
            if (_headers[i] == name)
            {
                location = i;
                return i;
            }
        }

        //Debug.Log("ERROR - no feature found with that name");
        return -1; // this only returns if faliar to find name
    }
    
    public string[] GetFeatureFromName(string name)
    {
        //first get the location int of the feature based on the name
        int location = LoactionFromName(name);

        string[] returnSting = new String[_manipulatedDataArray.GetLength(0)]; // create a new array the size of the number of instances in the dataset
        for (int i = 0; i < _manipulatedDataArray.GetLength(0) - 1; i++)
        {
            returnSting[i] = _manipulatedDataArray[i, location]; // set the return list to the values from the full data array
        }

        return returnSting;
    }

    public void ResetFeatureFromName(string name)
    {
        //Debug.Log("Reset " + name);
        //first get the location int of the feature based on the name
        int location = LoactionFromName(name);

        for (int i = 0; i < _manipulatedDataArray.Length; i++) // for each instance in the list of feature
        {
            _manipulatedDataArray[i, location] = _originalDataArray[i, location];
        }
    }

    public void ChangeFeaturesForName(string name, string[] manipulatedFeatures)
    {
        //Debug.Log("change for name");
        //first get the location int of the feature based on the name
        int location = LoactionFromName(name);

        for (int i = 0; i < manipulatedFeatures.Length; i++) // for each instance in the list of feature
        {
            _manipulatedDataArray[i, location] = manipulatedFeatures[i]; // change that feature arcording to the list
        }
    }
}