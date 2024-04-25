using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class csvCleaner : MonoBehaviour
{
    public TextAsset selectedFile;
    string[,] arr1 =  { { "1", "2" }, { "a", "b" }, { "4", "6" }, { "c", "d" } };
    
    [ContextMenu("Clean CSV")]
    private void Start()
    {
        GetFile();
    }

    
    public void GetFile()
    {
        string path = Application.dataPath + "\\RawCSVs\\" + selectedFile.name + ".csv";

        string[] headers;
        string[,] dataMatrix = ReadCSVFile(path, out headers);
        //ParseData(dataMatrix);
        SplitFile(dataMatrix);
    }

    public void SplitFile(string[,] _data)
    {
        int nRows = _data.GetLength(0);
        int nCols = _data.GetLength(1);

        float[,] _numericData = new float[nRows,nCols];
        string[,] _categoricData = new string[nRows,nCols];
        float[,] _convertedCategoricalData = new float[nRows, nCols];
        for (int row = 0; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                if(_data[row,col] == null) continue;
                
                float tmp;
                if (float.TryParse(_data[row, col], out tmp))
                {
                    _numericData[row, col] = tmp;
                }
                else
                {
                    _categoricData[row, col] = _data[row, col];
                }
            }
        }
        _convertedCategoricalData = ConvertToNum(_categoricData);
        NormalizeData(_numericData);
        SaveMatrixToCSV(CheckHandleNull(MergeMatrices(_numericData, _convertedCategoricalData)), Application.dataPath + "\\ModifiedCSVs\\" + selectedFile.name + ".csv");
    }

    public static float[,] ConvertToNum(string[,] _data)
    {
        int _rows = _data.GetLength(0);
        int _cols = _data.GetLength(1);

        float[,] _dataConverted = new float[_rows, _cols];

        List<Dictionary<string, float>> mapping = new List<Dictionary<string, float>>();

        for (int col = 0; col < _cols; col++)
        {
            mapping.Add(new Dictionary<string, float>());
        }

        for (int col = 0; col < _cols; col++)
        {
            HashSet<string> uniqueValuesSet = new HashSet<string>();

            for (int row = 0; row < _rows; row++)
            {
                uniqueValuesSet.Add(_data[row, col]);
            }

            List<string> uniqueValuesList = new List<string>(uniqueValuesSet);

            int count = uniqueValuesList.Count;
            float step = 1.0f / (count - 1);

            for (int i = 0; i < uniqueValuesList.Count; i++)
            {
                string currentValue = uniqueValuesList[i];
                float normalizeCurrentValue = i * step;
                if(uniqueValuesList[i] == null) continue;
                mapping[col].Add(currentValue, normalizeCurrentValue);
            }

            for (int row = 0; row < _rows; row++)
            {
                string currentValue = _data[row, col];
                if(currentValue == null) continue;
                _dataConverted[row, col] = mapping[col][currentValue];
            }
        }

        return _dataConverted;
    }

    public static float[,] MergeMatrices(float[,] array1, float[,] array2)
    {
        // Get dimensions of the arrays
        int rows1 = array1.GetLength(0);
        int cols1 = array1.GetLength(1);
        int rows2 = array2.GetLength(0);
        int cols2 = array2.GetLength(1);

        // Create a new array to hold the merged result
        float[,] mergedArray = new float[rows1, cols1 + cols2];

        // Copy elements from the first array to the merged array
        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols1; j++)
            {
                mergedArray[i, j] = array1[i, j];
            }
        }

        // Copy elements from the second array to the merged array
        for (int i = 0; i < rows2; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                mergedArray[i, j + cols1] = array2[i, j];
            }
        }

        return mergedArray;
    }
    
    public void ParseData(string[,] _data)
    {
        int nRows = _data.GetLength(0);
        int nCols = _data.GetLength(1);
        float[,] newData = new float[nRows,nCols];
        for (int row = 0; row < nRows; row++)
        {
            for (int collumns = 0; collumns < nCols; collumns++)
            {
                newData[row, collumns] = float.Parse(_data[row, collumns]);
            }
        }
    }

    
    public void RemoveCount(float[,] _data)
    {
        int columnToCheck = 0;
        int nRows = _data.GetLength(0);
        int nCols = _data.GetLength(1);
        
        float[,] newMatrix = new float[nRows, nCols];

        for (int i = 0; i < nRows-1; i++)
        {
            int k = 0;
            for (int j = 0; j < nCols-1; j++)
            {
                if (_data[i + 1, 0] - 1 != _data[i, 0]) continue;
                if (j == columnToCheck) continue;
                newMatrix[i, k++] = _data[i, j];
                
            }
        }
        // Check if all values in newMatrix are 0
        bool allZeros = true;
        foreach (float value in newMatrix)
        {
            if (value != 0)
            {
                allZeros = false;
                break;
            }
        }

        // If all values in newMatrix are 0, assign newMatrix to be equal to _data
        if (allZeros)
        {
            newMatrix = _data;
        }
        CheckHandleNull(newMatrix);
    }

    public float[,] CheckHandleNull(float[,] _data)
    {
        int nRows = _data.GetLength(0);
        int nCols = _data.GetLength(1);
        for (int i = 0; i < nRows; i++)
        {
            for (int j = 0; j < nCols; j++)
            {
                if (float.IsNaN(_data[i, j]))
                {
                    _data[i, j] = FindMode(_data);
                }
                
            }
        }
        //Debug.Log(_data[3,2]);
        //NormalizeData(_data);
        return _data;
    }

    float FindMode(float[,] _data)
    {
        Dictionary<float, int> valCounts = new Dictionary<float, int>();

        foreach (float value in _data)
        {
            if (valCounts.ContainsKey(value)) valCounts[value]++;
            else valCounts[value] = 1;
        }

        float mode = 0;
        int count = 0;
        foreach (KeyValuePair<float, int> vals in valCounts)
        {
            mode = vals.Key;
            count = vals.Value;
        }

        return mode;
    }

    public void NormalizeData(float[,] _data)
    {

        int rows = _data.GetLength(0);
        int cols = _data.GetLength(1);
        Debug.Log(_data[0,0]);
        float[] minValues = new float[cols];
        float[] maxValues = new float[cols];
        
        for (int j = 0; j < cols; j++)
        {
            minValues[j] = _data[0, j];
            maxValues[j] = _data[0, j];

            for (int i = 1; i < rows; i++)
            {
                minValues[j] = Mathf.Min(minValues[j], _data[i, j]);
                maxValues[j] = Mathf.Max(maxValues[j], _data[i, j]);
            }
        }

        for (int j = 0; j < cols; j++)
        {
            float columnMin = minValues[j];
            float columnMax = maxValues[j];

            for (int i = 0; i < rows; i++)
            {
                _data[i, j] = (_data[i, j] - columnMin) / (columnMax - columnMin);
            }
        }
        //SaveMatrixToCSV(_data, Application.dataPath + "\\ModifiedCSVs\\" + selectedFile.name + ".csv");
    }
    
    public void SaveMatrixToCSV(float[,] matrix, string filePath)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < rows; i++)
            {
                string[] rowData = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                    rowData[j] = matrix[i, j].ToString();
                }
                writer.WriteLine(string.Join(",", rowData));
            }
        }

        Debug.Log("Matrix saved to CSV: " + filePath);
    }


    string[,] ReadCSVFile(string path, out string[] headers) // function for reading the CSV file
    {
        string[] lines = File.ReadAllLines(path); // an array of all rows and all text in them exe: (patient0,123,4,56,yes,78,true,9)

        // Assuming that the CSV file has rows and columns
        int numRows = lines.Length; // number of
        int numCols = lines[0].Split(',').Length;

        headers = new string[numCols];
        string[,] dataMatrix = new string[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            string[] values = lines[i].Split(',');
            if (i == 0)
            {
                for (int j = 0; j < numCols; j++)
                {
                    headers[j] = values[j];
                }
            }
            else
            {
                for (int j = 0; j < numCols; j++)
                {
                    dataMatrix[i-1, j] = values[j];
                }
            }
        }
        return dataMatrix;
    }
}
