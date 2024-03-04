using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureObject : MonoBehaviour
{
    public FeatureObjectsHandeler featureObjectsHandeler;
    public Vector3 boardPosition;

    public void AssignedToDimention()
    {
        
    }

    public void LetGoOfObject()
    {
        transform.position = boardPosition;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
