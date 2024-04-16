using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slider : MonoBehaviour
{
    public FeatureManipulation featureManipulation;
    
    public GameObject minSliderHandle;
    public GameObject maxSliderHandle;

    public Transform rightHand;
    public Transform leftHand;

    public float minDistance = 0.1f;

    public TMP_Text minText;
    public TMP_Text maxText;
    
    private float _minStartingX;
    private float _maxStartingX;
    private bool _isMinSet = false;
    private bool _isMaxSet = false;
    
    
    private bool _minIsMoving = false;
    private bool _maxIsMoving = false;

    private Transform _handOnMax;
    private Transform _handOnMin;

    // Start is called before the first frame update
    void Start()
    {
        _minStartingX = minSliderHandle.transform.position.x;
        _maxStartingX = maxSliderHandle.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        minText.text = NormalizeValue(minSliderHandle.transform.position.x).ToString("F3");
        maxText.text = NormalizeValue(maxSliderHandle.transform.position.x).ToString("F3");
        
        if (_maxIsMoving && _handOnMax.transform.position.x <= _maxStartingX 
                         && _handOnMax.transform.position.x >=  minSliderHandle.transform.position.x + minDistance)
        {
            if (!_isMaxSet)
            {
                _maxStartingX = maxSliderHandle.transform.position.x;
                _isMaxSet = true;
            }
            
            maxSliderHandle.transform.position = new Vector3(_handOnMax.transform.position.x, 
                maxSliderHandle.transform.position.y, maxSliderHandle.transform.position.z);
        }

        if (_minIsMoving && _handOnMin.transform.position.x >= _minStartingX 
                         && _handOnMin.transform.position.x <= maxSliderHandle.transform.position.x - minDistance)
        {
            if (!_isMinSet)
            {
                _minStartingX = minSliderHandle.transform.position.x;
                _isMinSet = true;
            }
            
            minSliderHandle.transform.position = new Vector3(_handOnMin.transform.position.x, 
                minSliderHandle.transform.position.y, minSliderHandle.transform.position.z);
        }
    }

    public void SetSliderValues(Vector2 setRange)
    {
        float nexMaxX = (_maxStartingX - _minStartingX) * setRange.y + _minStartingX;
        float nexMinX = (_maxStartingX - _minStartingX) * setRange.x + _minStartingX;
        
        maxSliderHandle.transform.localPosition = new Vector3(nexMaxX,
            maxSliderHandle.transform.localPosition.y,
            maxSliderHandle.transform.localPosition.z);
        
        minSliderHandle.transform.localPosition = new Vector3(nexMinX,
            minSliderHandle.transform.localPosition.y,
            minSliderHandle.transform.localPosition.z);
    }

    public void MoveSlider(bool maxSlider)
    {
        GameObject handThatGrabed = maxSlider
            ? maxSliderHandle.transform.GetChild(1).gameObject
            : minSliderHandle.transform.GetChild(1).gameObject;


        if (maxSlider)
            _maxIsMoving = true;
        else
            _minIsMoving = true;



        switch (handThatGrabed.name)
        {
            case "[Right Controller] Dynamic Attach":
                if (maxSlider)
                    _handOnMax = rightHand.transform;
                else
                    _handOnMin = rightHand.transform;
                break;
            
            case "[Left Controller] Dynamic Attach":
                if (maxSlider)
                    _handOnMax = leftHand.transform;
                else
                    _handOnMin = leftHand.transform;
                break;
        }
    }

    public void LetGoOfSlider(bool maxSlider)
    {
        if (maxSlider)
        {
            _maxIsMoving = false;
            featureManipulation.maxSliderCurrentValue = NormalizeValue(maxSliderHandle.transform.position.x);
        }
        else
        {
            _minIsMoving = false;
            featureManipulation.minSliderCurrentValue = NormalizeValue(minSliderHandle.transform.position.x);
        }
            
    }
    
    // This method normalizes a value between minValue and maxValue to a range between 0 and 1
    private float NormalizeValue(float value)
    {
        return (value - _minStartingX) / (_maxStartingX - _minStartingX);
    }
}
