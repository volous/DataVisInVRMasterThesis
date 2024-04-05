using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slider : MonoBehaviour
{
    public GameObject minSliderHandle;
    public GameObject maxSliderHandle;

    public Transform rightHand;
    public Transform leftHand;

    public float minDistance = 0.1f;
    
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
        _minStartingX = 0;
         _maxStartingX = maxSliderHandle.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_maxIsMoving && _handOnMax.transform.position.x <= _maxStartingX 
                         && _handOnMax.transform.position.x >=  minSliderHandle.transform.localPosition.x + minDistance)
        {
            if (!_isMaxSet)
            {
                _maxStartingX = maxSliderHandle.transform.localPosition.x;
                _isMaxSet = true;
            }
            
            maxSliderHandle.transform.localPosition = new Vector3(_handOnMax.transform.position.x, 
                maxSliderHandle.transform.localPosition.y, maxSliderHandle.transform.localPosition.z);
        }

        if (_minIsMoving && _handOnMin.transform.position.x >= _minStartingX 
                         && _handOnMin.transform.position.x <= maxSliderHandle.transform.localPosition.x - minDistance)
        {
            if (!_isMinSet)
            {
                _minStartingX = minSliderHandle.transform.localPosition.x;
                _isMinSet = true;
            }
            
            minSliderHandle.transform.localPosition = new Vector3(_handOnMin.transform.position.x, 
                minSliderHandle.transform.localPosition.y, minSliderHandle.transform.localPosition.z);
        }
    }

    public void MoveSlider(bool maxSlider)
    {
        GameObject handThatGrabed = maxSlider
            ? maxSliderHandle.transform.GetChild(0).gameObject
            : minSliderHandle.transform.GetChild(0).gameObject;


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
            _maxIsMoving = false;
        else
            _minIsMoving = false;
    }
    
}
