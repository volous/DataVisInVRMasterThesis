using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


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
        foreach (var value in _data)
        {
            if (value == 0 || value == null)
            {
                
            }
        }
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
