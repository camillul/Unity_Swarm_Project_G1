using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PLAY()
    {
        SceneManager.LoadScene("Drone", LoadSceneMode.Single);
    }

    public void CLOSE()
    {
        SceneManager.LoadScene("Drone", LoadSceneMode.Single);
        Application.Quit();
    }


}