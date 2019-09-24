using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelect : MonoBehaviour
{
    public GameObject Runner;
    public GameObject Poke;
    public GameObject selected;
    // Start is called before the first frame update
    void Start()
    {
        ControllerStateMachine.Instance.StopGame();
        selected = Runner;
        Runner.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) && selected == Runner)
        {
            selected = Poke;
            Poke.GetComponent<Image>().color = Color.green;
            Runner.GetComponent<Image>().color = Color.white;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && selected == Poke)
        {
            selected = Runner;
            Poke.GetComponent<Image>().color = Color.white;
            Runner.GetComponent<Image>().color = Color.green;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            float randValue = Random.value;
            State currState = ControllerStateMachine.Instance.currentState;
            if(currState.listenPercent > randValue){
                randValue = Random.value;
                if(selected == Runner)
                {
                    if(currState.wrongInputPercent > randValue){
                        SceneManager.LoadScene("BattleMinigame");
                    }
                    else{
                        SceneManager.LoadScene("RunnerMinigame");    
                    }
                    
                }

                if(selected == Poke)
                {
                    
                    if(currState.wrongInputPercent > randValue){
                        SceneManager.LoadScene("RunnerMinigame");
                    }
                    else{
                        SceneManager.LoadScene("BattleMinigame");    
                    }
                }
            }
            
        }
    }
}
