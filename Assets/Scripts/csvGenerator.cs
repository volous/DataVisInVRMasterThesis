using UnityEngine;
using System.Collections;
using System.IO;

public class csvGenerator : MonoBehaviour
{
    public string csvFilePath; // Specify the path to save the generated CSV file
    public int numDimensions = 10;
    public int numRows = 10; // Adjust the number of rows as needed

    void Start()
    {
        
    }

    [ContextMenu("Start Generate CSV")]
    public void StartGenerate()
    {
        string path = csvFilePath + "\\file.csv";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        GenerateCSVFile(path);
        Debug.Log("CSV file generated at: " + csvFilePath);
    }

    void GenerateCSVFile(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            // // Write header row
            // writer.Write("Dimension 1");
            // for (int i = 2; i <= numDimensions; i++)
            // {
            //     writer.Write($", Dimension {i}");
            // }
            //
            // for (int i = 1; i <= numVariables; i++)
            // {
            //     writer.Write($", Variable {i}");
            // }

            //writer.WriteLine();

            // Write data rows
            for (int row = 1; row <= numRows; row++)
            {
                writer.Write(RandomFloat());
                for (int col = 2; col <= numDimensions; col++)
                {
                    writer.Write($",{RandomFloat()}");
                }
                writer.WriteLine();
            }
        }
    }

    float RandomFloat()
    {
        return Mathf.Round(Random.Range(0f, 1f) * 1000f) / 1000f;

    }
}