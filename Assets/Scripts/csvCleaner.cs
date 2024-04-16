using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class csvCleaner : MonoBehaviour
{
    public TextAsset selectedFile;
    
    private void Start()
    {
        GetFile();
    }

    [ContextMenu("Clean CSV")]
    public void GetFile()
    {
        string path = Application.dataPath + "\\RawCSVs\\" + selectedFile.name + ".csv";

        string[] headers;
        string[,] dataMatrix = ReadCSVFile(path, out headers);
        ParseData(dataMatrix);
    }

    public void ParseData(string[,] _data)
    {
        int nRows = _data.GetLength(0) - 1;
        int nCols = _data.GetLength(1) - 1;
        float[,] newData = new float[nRows,nCols];
        for (int row = 0; row < nRows; row++)
        {
            for (int collumns = 0; collumns < nCols; collumns++)
            {
                newData[row, collumns] = float.Parse(_data[row, collumns]);
            }
        }
        RemoveCount(newData);
    }
    
    public void RemoveCount(float[,] _data)
    {
        int columnToCheck = 0;
        int nRows = _data.GetLength(0) - 1;
        int nCols = _data.GetLength(1) - 1;
        
        float[,] newMatrix = new float[nRows, nCols];

        for (int i = 0; i < nRows; i++)
        {
            int k = 0;
            for (int j = 0; j < nCols; j++)
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

    public void CheckHandleNull(float[,] _data)
    {
        Debug.Log(_data[3,2]);
        int nRows = _data.GetLength(0) - 1;
        int nCols = _data.GetLength(1) - 1;
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
        Debug.Log(_data[3,2]);
        NormalizeData(_data);
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
        SaveMatrixToCSV(_data, Application.dataPath + "\\ModifiedCSVs\\" + selectedFile.name + ".csv");
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
