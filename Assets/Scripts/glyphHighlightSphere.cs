using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glyphHighlightSphere : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            Destroy(gameObject);
        }
    }
}
