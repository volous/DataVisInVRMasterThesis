using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class csvReader : MonoBehaviour
{
    public string csvFilePath; // Specify the path to your CSV file
    
    private DataPointsRenderer DPR;
    void Start()
    {
        DPR = this.gameObject.GetComponent<DataPointsRenderer>();
    }

    [ContextMenu("Start Read CSV")]
    public void StartRead()
    {
        string path = csvFilePath + "\\file.csv";
        
        // Read the CSV file and create the array
        string[] headers;
        string[,] dataArray = ReadCSVFile(path, out headers);
        
        for (int i = 0; i < headers.Length; i++)
        {
            Debug.Log(headers[i]);
        }
        
        InstaciateCubes(dataArray);
        
    }
    
    void InstaciateCubes(string[,] dataArray)
    {
        DPR.ReciveDataMatrix(dataArray);
    }

    string[,] ReadCSVFile(string path, out string[] headers)
    {
        string[] lines = File.ReadAllLines(path);

        // Assuming that the CSV file has rows and columns
        int numRows = lines.Length;
        int numCols = lines[0].Split(',').Length;

        headers = new string[numCols];
        string[,] dataArray = new string[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            string[] values = lines[i].Split(',');

            if (i == 0)
            {
                for (int j = 0; j < 1; j++)
                {
                    headers[i] = values[j];
                }
            }
            else
            {
                for (int j = 0; j < numCols; j++)
                {
                    dataArray[i-1, j] = values[j];
                }
            }
            
            
        }
        
        return dataArray;
    }
}