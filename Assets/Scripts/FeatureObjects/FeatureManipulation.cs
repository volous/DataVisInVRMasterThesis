using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManipulation : MonoBehaviour
{
    public DataPointsRenderer DPR;

    private string[] _dataInFeature;
    
    [ContextMenu("Data from DRP")]
    public void DataFromDPR(string name)
    {
        _dataInFeature = DPR.GetFeatureFromName(name);

        foreach (string s in _dataInFeature)
        {
            Debug.Log(s);
        }
    }
}
