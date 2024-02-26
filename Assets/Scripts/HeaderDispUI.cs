using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeaderDispUI : MonoBehaviour
{
    public FeatureGrabbing featureGrabbing;

    public float xSpacing;
    public float ySpacing;
    
    //String array for getting names of headers/features
    private string[] featureList;

    private List<Button> _newFeatureList;
    //get canvas
    [SerializeField] private Canvas _handUICanvas;
    //getting object for inserting it into the UI, should grabbable be canvas within canvas, button or text?
    [SerializeField] private Button _buttonPrefab;

    private void Start()
    {
        _newFeatureList = new List<Button>();
    }

    //function for initial population of features
    private void InitFeatureOnCanvas()
    {

        if (_newFeatureList.Count > 0)
        {
            foreach (Button button in _newFeatureList)
            {
                Destroy(button.gameObject);
            }
            
            _newFeatureList.Clear();
        }
       
        if (featureList.Length != 0)
        {
            int xCounter = 0;
            
            for (int i = 0; i < featureList.Length; i++)
            {
                
                //create objects based on how many features there are in the string list
                Button _newFeature = Instantiate(_buttonPrefab);
                _newFeature.transform.SetParent(_handUICanvas.transform, false);
                //text on button is set to the list of features
                _newFeature.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = featureList[i];
                _newFeature.name = featureList[i];
                _newFeature.onClick.AddListener(() => featureGrabbing.ChosenButton(_newFeature));
                _newFeature.transform.localPosition = Vector3.zero;
                _newFeatureList.Add(_newFeature);
                //set positions, if amount of features is 9(some amount) or greater, start moving the features in the x direction
                if (i % 9 == 0 && i!=0)
                {
                    xCounter ++;
                }
                
                
                _newFeatureList[i].transform.localPosition = new Vector3(
                    -4 + xCounter * (xSpacing * i), 
                    4 - ySpacing * i, 
                    -0.5f);
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
