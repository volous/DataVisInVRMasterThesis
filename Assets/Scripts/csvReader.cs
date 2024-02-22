using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class csvReader : MonoBehaviour
{
    private DataPointsRenderer DPR; // DataPointsRenderer is a class for handeling the visualization of the data
    void Start()
    {
        DPR = this.gameObject.GetComponent<DataPointsRenderer>(); // find the DPR class, located on this gameobject
    }

    [ContextMenu("Start Read CSV")] // alowes for the function to run from the editor
    public void StartRead() 
    {
        string path = Application.dataPath + "\\CSVs\\file.csv"; // the path location to the file called file, in the CVSs folder in the assets folder
        
        
        string[] headers; // array of strings for the top line, aka the headers
        string[,] dataArray = ReadCSVFile(path, out headers); // a string Matrix for the rest of the data file
        
        DPR.ReciveDataMatrix(dataArray, headers); // calles the rendering function in the DataRenderer
        
    }
    
    string[,] ReadCSVFile(string path, out string[] headers) // function for reading the CSV file
    {
        string[] lines = File.ReadAllLines(path); // an array of all rows and all text in them exe: (patient0,123,4,56,yes,78,true,9)

        // Assuming that the CSV file has rows and columns
        int numRows = lines.Length; // number of
        int numCols = lines[0].Split(',').Length;

        headers = new string[numCols];
        string[,] dataArray = new string[numRows, numCols];

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
                    dataArray[i-1, j] = values[j];
                }
            }
            
            
        }
        
        return dataArray;
    }
}