using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class csvCleaner : MonoBehaviour
{
    // public TextAsset selectedFile;
    // string[,] arr1 =  { {"Header1" , "1", "2" }, {"Header2" , "a", "b" }, {"Header3" , "4", "6" }, {"Header4" , "c" , "d" } };
    //
    // [ContextMenu("Clean CSV")]
    // private void Start()
    // {
    //     GetFile();
    // }
    //
    //
    //
    // public void GetFile()
    // {
    //     string path = Application.dataPath + "\\RawCSVs\\" + selectedFile.name + ".csv";
    //     int rowCount = arr1.GetLength(0);
    //     string[] headers = new string[rowCount];
    //
    //     // for (int i = 0; i < rowCount; i++)
    //     // {
    //     //     headers[i] = arr1[i, 0];
    //     // }
    //
    //     string[,] dataMatrix = ReadCSVFile(path, out headers);
    //     //ParseData(dataMatrix);
    //     SplitFile(dataMatrix, headers);
    // }
    //
    // string[,] ReadCSVFile(string path, out string[] headers) // function for reading the CSV file
    // {
    //     string[] lines = File.ReadAllLines(path); // an array of all rows and all text in them exe: (patient0,123,4,56,yes,78,true,9)
    //
    //     // Assuming that the CSV file has rows and columns
    //     int numRows = lines.Length; // number of
    //     int numCols = lines[0].Split(',').Length;
    //
    //     headers = new string[numCols];
    //     string[,] dataMatrix = new string[numRows, numCols];
    //
    //     for (int i = 0; i < numRows; i++)
    //     {
    //         string[] values = lines[i].Split(',');
    //         if (i == 0)
    //         {
    //             for (int j = 0; j < numCols; j++)
    //             {
    //                 headers[j] = values[j];
    //             }
    //         }
    //         else
    //         {
    //             for (int j = 0; j < numCols; j++)
    //             {
    //                 dataMatrix[i-1, j] = values[j];
    //             }
    //         }
    //     }
    //     return dataMatrix;
    // }
    //
    // public void SplitFile(string[,] _data, string[] headers)
    // {
    //     // Debug.Log(headers.Length);
    //     int nRows = _data.GetLength(0);
    //     int nCols = _data.GetLength(1);
    //
    //     float[,] _numericData = new float[nRows,nCols];
    //     string[,] _categoricData = new string[nRows,nCols];
    //     string[,] _convertedCategoricalData = new string[nRows, nCols];
    //
    //     string[] numericalHeaderPosition = new string[nCols];
    //     string[] categoricalHeaderPosition = new string[nCols];
    //
    //     int numericCounter = 0;
    //     int categoricalCounter = 0;
    //     for (int row = 0; row < nRows; row++)
    //     {
    //         for (int col = 0; col < nCols; col++)
    //         {
    //             
    //             if(_data[row,col] == null) continue;
    //             
    //             float tmp;
    //             if (float.TryParse(_data[row, col], out tmp))
    //             {
    //                 if (row == 0)
    //                 {
    //                     numericalHeaderPosition[col] = headers[numericCounter];
    //                     numericCounter += 1;
    //                 }
    //                 _numericData[row, col] = tmp;
    //             }
    //             else
    //             {
    //                 if (row == 0)
    //                 {
    //                     categoricalHeaderPosition[col] = headers[categoricalCounter];
    //                     categoricalCounter += 1;
    //                 }
    //                 _categoricData[row, col] = _data[row, col];
    //             }
    //         }
    //     }
    //     // Debug.Log(numericalHeaderPosition.Count);
    //     // Debug.Log(categoricalHeaderPosition.Count);
    //     _convertedCategoricalData = ConvertToNum(_categoricData);
    //     SaveMatrixToCSV(CheckHandleNull(MergeMatrices(NormalizeData(_numericData), _convertedCategoricalData, numericalHeaderPosition, categoricalHeaderPosition)), Application.dataPath + "\\ModifiedCSVs\\" + selectedFile.name + ".csv");
    // }
    //
    // public static string[,] ConvertToNum(string[,] _data)
    // {
    //     int _rows = _data.GetLength(0);
    //     int _cols = _data.GetLength(1);
    //
    //     float[,] _dataConverted = new float[_rows, _cols];
    //
    //     List<Dictionary<string, float>> mapping = new List<Dictionary<string, float>>();
    //
    //     for (int col = 0; col < _cols; col++)
    //     {
    //         mapping.Add(new Dictionary<string, float>());
    //     }
    //
    //     for (int col = 0; col < _cols; col++)
    //     {
    //         HashSet<string> uniqueValuesSet = new HashSet<string>();
    //
    //         for (int row = 0; row < _rows; row++)
    //         {
    //             uniqueValuesSet.Add(_data[row, col]);
    //         }
    //
    //         List<string> uniqueValuesList = new List<string>(uniqueValuesSet);
    //
    //         int count = uniqueValuesList.Count;
    //         float step = 1.0f / (count - 1);
    //
    //         for (int i = 0; i < uniqueValuesList.Count; i++)
    //         {
    //             string currentValue = uniqueValuesList[i];
    //             float normalizeCurrentValue = i * step;
    //             if(uniqueValuesList[i] == null) continue;
    //             mapping[col].Add(currentValue, normalizeCurrentValue);
    //         }
    //
    //         for (int row = 0; row < _rows; row++)
    //         {
    //             string currentValue = _data[row, col];
    //             if(currentValue == null) continue;
    //             _dataConverted[row, col] = mapping[col][currentValue];
    //         }
    //     }
    //
    //     string[,] _dataReConverted = new string[_dataConverted.GetLength(0), _dataConverted.GetLength(1)];
    //     
    //     for (int row = 0; row < _dataConverted.GetLength(0); row++)
    //     {
    //         for (int col = 0; col < _dataConverted.GetLength(1); col++)
    //         {
    //             _dataReConverted[row, col] = _dataConverted[row, col].ToString();
    //         }
    //     }
    //
    //     return _dataReConverted;
    // }
    //
    // public static string[,] MergeMatrices(string[,] array1, string[,] array2, string[] numericHeaders, string[] categoricalHeaders)
    // {
    //     // Get dimensions of the arrays
    //     int rows1 = array1.GetLength(0);
    //     int cols1 = array1.GetLength(1);
    //     int rows2 = array2.GetLength(0);
    //     int cols2 = array2.GetLength(1);
    //     int mergedCollumnCount = cols1 + cols2;
    //     
    //     
    //     
    //     // Create a new array to hold the merged result
    //     string[,] mergedArray = new String[rows1, cols1 + cols2];
    //     
    //     
    //     
    //     // Copy elements from the first array to the merged array
    //     for (int i = 0; i < rows1; i++)
    //     {
    //         for (int j = 0; j < cols1; j++)
    //         {
    //             if (i == 0)
    //             {
    //                 mergedArray[0, j] = numericHeaders[j];
    //             }
    //             mergedArray[i, j] = array1[i, j].ToString();
    //         }
    //     }
    //
    //     // Copy elements from the second array to the merged array
    //     for (int i = 0; i < rows2; i++)
    //     {
    //         for (int j = 0; j < cols2; j++)
    //         {
    //             if (i == 0)
    //             {
    //                 mergedArray[0, j] = categoricalHeaders[j];
    //             }
    //             mergedArray[i, j + cols1] = array2[i, j].ToString();
    //         }
    //     }
    //     
    //     return mergedArray;
    // }
    //
    // public void ParseData(string[,] _data)
    // {
    //     int nRows = _data.GetLength(0);
    //     int nCols = _data.GetLength(1);
    //     float[,] newData = new float[nRows,nCols];
    //     for (int row = 0; row < nRows; row++)
    //     {
    //         for (int collumns = 0; collumns < nCols; collumns++)
    //         {
    //             newData[row, collumns] = float.Parse(_data[row, collumns]);
    //         }
    //     }
    // }
    //
    //
    // public void RemoveCount(float[,] _data)
    // {
    //     int columnToCheck = 0;
    //     int nRows = _data.GetLength(0);
    //     int nCols = _data.GetLength(1);
    //     
    //     float[,] newMatrix = new float[nRows, nCols];
    //
    //     for (int i = 0; i < nRows-1; i++)
    //     {
    //         int k = 0;
    //         for (int j = 0; j < nCols-1; j++)
    //         {
    //             if (_data[i + 1, 0] - 1 != _data[i, 0]) continue;
    //             if (j == columnToCheck) continue;
    //             newMatrix[i, k++] = _data[i, j];
    //             
    //         }
    //     }
    //     // Check if all values in newMatrix are 0
    //     bool allZeros = true;
    //     foreach (float value in newMatrix)
    //     {
    //         if (value != 0)
    //         {
    //             allZeros = false;
    //             break;
    //         }
    //     }
    //
    //     // If all values in newMatrix are 0, assign newMatrix to be equal to _data
    //     if (allZeros)
    //     {
    //         newMatrix = _data;
    //     }
    //     CheckHandleNull(newMatrix);
    // }
    //
    // public string[,] CheckHandleNull(string[,] _data)
    // {
    //     int nRows = _data.GetLength(0);
    //     int nCols = _data.GetLength(1);
    //     for (int i = 0; i < nRows; i++)
    //     {
    //         for (int j = 0; j < nCols; j++)
    //         {
    //             if(_data[i,j] == null) continue;
    //             // if (float.IsNaN(_data[i, j]))
    //             // {
    //             //     _data[i, j] = FindMode(_data);
    //             // }
    //             //
    //         }
    //     }
    //
    //     string[,] convString = new string[_data.GetLength(0), _data.GetLength(1)];
    //     for (int i = 0; i < nRows; i++)
    //     {
    //         for (int j = 0; j < nCols; j++)
    //         {
    //
    //             convString[i,j] = _data[i, j].ToString();
    //
    //         }
    //     }
    //     //Debug.Log(_data[3,2]);
    //     //NormalizeData(_data);
    //     return convString;
    // }
    //
    // float FindMode(float[,] _data)
    // {
    //     Dictionary<float, int> valCounts = new Dictionary<float, int>();
    //
    //     foreach (float value in _data)
    //     {
    //         if (valCounts.ContainsKey(value)) valCounts[value]++;
    //         else valCounts[value] = 1;
    //     }
    //
    //     float mode = 0;
    //     int count = 0;
    //     foreach (KeyValuePair<float, int> vals in valCounts)
    //     {
    //         mode = vals.Key;
    //         count = vals.Value;
    //     }
    //
    //     return mode;
    // }
    //
    // public string[,] NormalizeData(float[,] _data)
    // {
    //
    //     int rows = _data.GetLength(0);
    //     int cols = _data.GetLength(1);
    //     float[] minValues = new float[cols];
    //     float[] maxValues = new float[cols];
    //     string[,] _dataReConverted = new string[rows, cols];
    //     for (int j = 0; j < cols; j++)
    //     {
    //         minValues[j] = _data[0, j];
    //         maxValues[j] = _data[0, j];
    //
    //         for (int i = 1; i < rows; i++)
    //         {
    //             minValues[j] = Mathf.Min(minValues[j], _data[i, j]);
    //             maxValues[j] = Mathf.Max(maxValues[j], _data[i, j]);
    //         }
    //     }
    //
    //     for (int j = 0; j < cols; j++)
    //     {
    //         float columnMin = minValues[j];
    //         float columnMax = maxValues[j];
    //
    //         for (int i = 0; i < rows; i++)
    //         {
    //             _data[i, j] = (_data[i, j] - columnMin) / (columnMax - columnMin);
    //         }
    //     }
    //
    //     for (int row = 0; row < rows; row++)
    //     {
    //         for (int col = 0; col < cols; col++)
    //         {
    //             _dataReConverted[row, col] = _data[row, col].ToString();
    //         }
    //     }
    //
    //     return _dataReConverted;
    //     // SaveMatrixToCSV(_dataReConverted, Application.dataPath + "\\ModifiedCSVs\\" + selectedFile.name + ".csv");
    // }
    //
    // public void SaveMatrixToCSV(string[,] matrix, string filePath)
    // {
    //     int rows = matrix.GetLength(0);
    //     int cols = matrix.GetLength(1);
    //
    //     using (StreamWriter writer = new StreamWriter(filePath))
    //     {
    //         for (int i = 0; i < rows; i++)
    //         {
    //             string[] rowData = new string[cols];
    //             for (int j = 0; j < cols; j++)
    //             {
    //                 rowData[j] = matrix[i, j].ToString();
    //             }
    //             writer.WriteLine(string.Join(",", rowData));
    //         }
    //     }
    // }
    //
    //
    //
}
