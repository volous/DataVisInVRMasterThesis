using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManipulation : MonoBehaviour
{
    public DataPointsRenderer DPR;
    
    private string[] _dataInFeature;

    public float maxSliderCurrentValue;
    public float minSliderCurrentValue;
    
    [ContextMenu("Data from DRP")]
    public void DataFromDPR(string name)
    {
        _dataInFeature = DPR.GetFeatureFromName(name);

        foreach (string s in _dataInFeature)
        {

        }
    }

    public void ManipulateData(string name, Vector2 range, FeatureObject featureObject)
    {
        int tst = -1;
        for (int i = 0; i < _dataInFeature.Length -1; i++)
        {
            if (float.Parse(_dataInFeature[i]) < range.x || float.Parse(_dataInFeature[i]) > range.y)
            {
                _dataInFeature[i] = tst.ToString();
            }
        }

        DPR.ChangeFeaturesForName(name, _dataInFeature);
        featureObject.featureRange = new Vector2( minSliderCurrentValue, maxSliderCurrentValue);
    }
}
