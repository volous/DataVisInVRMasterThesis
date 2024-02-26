using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeaderDispUI : MonoBehaviour
{
    public FeatureGrabbing featureGrabbing;
    
    //String array for getting names of headers/features
    private string[] featureList;

    //get canvas
    [SerializeField] private Canvas _handUICanvas;
    //getting object for inserting it into the UI, should grabbable be canvas within canvas, button or text?
    [SerializeField] private Button _buttonPrefab;
    
    //function for initial population of features
    private void InitFeatureOnCanvas()
    {
        List<Button> _newFeatureList = new List<Button>();
        if (featureList.Length != 0)
        {
            float xMult = 1;
            int ySub = 0;
            for (int i = 0; i < featureList.Length; i++)
            {
                
                //create objects based on how many features there are in the string list
                Button _newFeature = Instantiate(_buttonPrefab);
                _newFeature.transform.SetParent(_handUICanvas.transform, false);
                //text on button is set to the list of features
                _newFeature.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = featureList[i];
                _newFeature.name = featureList[i];
                _newFeature.onClick.AddListener(() => featureGrabbing.ChosenButton(_newFeature));
                _newFeatureList.Add(_newFeature);
                //set positions, if amount of features is 9(some amount) or greater, start moving the features in the x direction
                if (i % 9 == 0 && i!=0)
                {
                    xMult += 1.5f;
                    ySub += 9;
                }
                _newFeatureList[i].transform.localPosition = new Vector3(-4+xMult, 4-(_newFeatureList[i].transform.position.y * i - (ySub * _newFeatureList[i].transform.position.y)), -0.5f);
            }
        }
    }

    public void ReceiveFeatures(string[] receivedFeatures)
    {
        featureList = new string[receivedFeatures.Length];
        featureList = receivedFeatures;
        InitFeatureOnCanvas();
    }
}
