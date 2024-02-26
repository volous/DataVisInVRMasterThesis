using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FeatureGrabbing : MonoBehaviour
{

    public DataPointsRenderer DPR;
    public Button[] buttons;

    private string[] _dimentions;
    private bool _isChoosing;
    private string _currentChoice;


    private void Start()
    {
        _isChoosing = false;
        _dimentions = new string[7];
    }

    public void ChosenButton(Button button)
    {
        _isChoosing = true;
        _currentChoice = button.name;
    }

    public void AssignChoice(int dimentionID)
    {
        if (_isChoosing && _currentChoice != null)
        {
            _dimentions[dimentionID] = _currentChoice;
            buttons[dimentionID].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = _currentChoice;

            _isChoosing = false;
            _currentChoice = null;

            DprRendering();
        }
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
        
        DPR.ReciveDataMatrix(_dimentions);
    }
}
