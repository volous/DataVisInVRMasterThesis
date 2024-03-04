using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureObjectsHandeler : MonoBehaviour
{
    public GameObject featureInteractionCube;
    public Vector3 firstPosition;
    public GameObject featureBoard;

        [Header("Positioning Varables")] 
    public float xSpacing;
    public float ySpacing;
    public int numberOfFeaturesOnY;
    
    private string[] _featureStrings;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstanciateInteractablesFromList()
    {
        int xMultyplier = 0;
        int yMultyplier = 0;

        Vector3 firstFeaturePosition = firstPosition;
        
        for (int featureString = 0; featureString < _featureStrings.Length; featureString++)
        {
            if (featureString % numberOfFeaturesOnY == 0 && featureString != 0)
            {
                xMultyplier++;
                yMultyplier = 0;
            }

            Vector3 newPosition = new Vector3(
                firstFeaturePosition.x + xSpacing * xMultyplier,
                firstFeaturePosition.y - ySpacing * yMultyplier,
                firstFeaturePosition.z);

            yMultyplier++;

            GameObject newInteractionCube = Instantiate(featureInteractionCube);

            newInteractionCube.transform.SetParent(featureBoard.transform);
            newInteractionCube.name = _featureStrings[featureString];
            newInteractionCube.transform.localPosition = newPosition;
            
            FeatureObject featureObject = newInteractionCube.GetComponent<FeatureObject>();
            featureObject.boardPosition = newPosition;
            featureObject.boardScaler = newInteractionCube.transform.localScale;
            featureObject.SetText(_featureStrings[featureString]);
            featureObject.feature = newInteractionCube.name;
            
        }
    }

    public void ReciveFeatureString(string[] featuresString)
    {
        _featureStrings = featuresString;
        InstanciateInteractablesFromList();
    }
}
