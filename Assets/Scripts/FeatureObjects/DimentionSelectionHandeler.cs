using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimentionSelectionHandeler : MonoBehaviour
{
    public DataPointsRenderer DPR;

    private string[] _dimentions;


    private void Start()
    {
        _dimentions = new string[7];
    }
    

    public void AssignChoice(string feature, int dimentionID)
    {
        _dimentions[dimentionID] = feature;

        DprRendering();
    }

    void DprRendering()
    {
        for (int i = 0; i < _dimentions.Length; i++)
        {
            if (_dimentions[i] == "" || _dimentions[i] == null)
            {
                return;
            }
                
        }
        Debug.Log(_dimentions.Length);
        DPR.ReciveDataMatrix(_dimentions);
    }
}
