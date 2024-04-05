using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManipulation : MonoBehaviour
{
    public DataPointsRenderer DPR;

    public Vector2 TestRange; // (DELETE THIS) for testing only
    
    private string[] _dataInFeature;
    
    [ContextMenu("Data from DRP")]
    public void DataFromDPR(string name)
    {
        _dataInFeature = DPR.GetFeatureFromName(name);

        foreach (string s in _dataInFeature)
        {

        }
    }

    public void ManipulateData(string name, Vector2 range)
    {
        int tst = -1;
        for (int i = 0; i < _dataInFeature.Length -1; i++)
        {
            if (float.Parse(_dataInFeature[i]) < range.x || float.Parse(_dataInFeature[i]) > range.y)
            {
                Debug.Log($"feature {name} had a value of {_dataInFeature[i]} and was change to -1");
                _dataInFeature[i] = tst.ToString();
            }
        }

        DPR.ChangeFeaturesForName(name, _dataInFeature);
    }
}
