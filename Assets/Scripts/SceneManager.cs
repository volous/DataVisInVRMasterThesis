using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1))
            LoadLevel(1);
        
        if (Input.GetKey(KeyCode.Keypad2))
            LoadLevel(2);
        
        if (Input.GetKey(KeyCode.Keypad3))
            LoadLevel(3);
    }

    void LoadLevel(int sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }
}
