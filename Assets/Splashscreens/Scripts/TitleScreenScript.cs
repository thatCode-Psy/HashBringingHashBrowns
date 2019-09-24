using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{
    float startTime;
    public float waitTime = 10.5f;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime >= waitTime){
            StartGame();
        }
    }

    public void StartGame(){
        SceneManager.LoadScene("GameSelect");
    }
}
