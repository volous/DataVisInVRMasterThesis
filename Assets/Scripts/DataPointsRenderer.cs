using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.IO;
using Random = UnityEngine.Random;

public class DataPointsRenderer : MonoBehaviour
{
    public int RederingArea;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [Range(0,1)] public float transparency;


    private string[,] _dataArray;
    private RenderParams _rp;
    private bool _isRunning;
    private Vector3[] _position;
    private float[] _scales;
    private Mesh[] _meshes;
    private Color[] _colors;
    private Material[] _materials;

    private int _featureSelectionNumber;

    private void Start()
    {
        _isRunning = false;
        _featureSelectionNumber = 0;
    }

    public void ReciveDataMatrix(string[,] dataArray)
    {
        _dataArray = dataArray;
        _featureSelectionNumber = 0;
        
        
        BeginRendering();
    }

    private void BeginRendering()
    {
        _isRunning = true;

        int nRows = _dataArray.GetLength(0);
        int nFeatures = _dataArray.GetLength(1);

        _featureSelectionNumber = _featureSelectionNumber >= nFeatures ? 0 : _featureSelectionNumber;

        _position = new Vector3[nRows];
        _scales = new float[nRows];
        _meshes = new Mesh[nRows];
        _colors = new Color[nRows];
        _materials = new Material[nRows];

        _rp = new RenderParams(_material);

        // positions
        for (int row = 0; row < nRows; row++)
        {
            _position[row] = new Vector3(
                float.Parse(_dataArray[row, (0 + _featureSelectionNumber) % nFeatures]) * RederingArea,
                float.Parse(_dataArray[row, (1 + _featureSelectionNumber) % nFeatures]) * RederingArea,
                float.Parse(_dataArray[row, (2 + _featureSelectionNumber) % nFeatures]) * RederingArea
            );
        }

        //scales
        for (int row = 0; row < nRows; row++)
        {
            _scales[row] = float.Parse(_dataArray[row, (3 + _featureSelectionNumber) % nFeatures]);
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
                float.Parse(_dataArray[row, (4 + _featureSelectionNumber) % nFeatures]) * 2,
                float.Parse(_dataArray[row, (5 + _featureSelectionNumber) % nFeatures]) * 2,
                float.Parse(_dataArray[row, (6 + _featureSelectionNumber) % nFeatures]) * 2,
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

    [ContextMenu("Feature Selection Change")]
    private void FeatureDisplaySelection()
    {
        _featureSelectionNumber++;
        BeginRendering();
    }
}