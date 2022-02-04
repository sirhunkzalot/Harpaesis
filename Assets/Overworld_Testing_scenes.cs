using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overworld_Testing_scenes : MonoBehaviour
{
 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(8);
        }
    }
    public void _RetrurnOverworld()
    {
        SceneManager.LoadScene(2);
    }
}
