using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State{
    public float listenPercent;
    public float randomInputPercent;
    public float wrongInputPercent;
    public State(float listenPercent, float randomInputPercent, float wrongInputPercent){
        this.listenPercent = listenPercent;
        this.randomInputPercent = randomInputPercent;
        this.wrongInputPercent = wrongInputPercent;
    }
}



public class ControllerStateMachine : MonoBehaviour
{

    public static State DEFAULT = new State(0.8f, 0.1f, 0.05f);
    public static State HAPPY = new State(0.95f, 0f, 0f);
    public static State EXCITED = new State(0.7f, 0.5f, 0.1f);
    public static State ANGRY = new State(0.6f, 0.2f, 0.15f);
    public static State SAD = new State(0.4f, 0f, 0.15f);
    public static State DEPRESSED = new State(0.2f, 0f, 0.1f);

    public Texture DefaultImage;
    public Texture HappyImage;
    public Texture ExcitedImage;
    public Texture AngryImage;
    public Texture SadImage;
    public Texture DepressedImage;

    float startTime;
    bool started;



    public float randomInputDelay = 5f;


    State currentState;

    public ControllerInterface currentGame;


    public static ControllerStateMachine Instance{
        get;
        set;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    void Start(){
        currentState = DEFAULT;
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(started){
            if(Time.time - startTime > randomInputDelay){
                startTime = Time.time;
                if(currentState.randomInputPercent > Random.value){
                    RandomInput();
                }
            }
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float a = Input.GetAxis("A");
            float b = Input.GetAxis("B");
            if(horizontal != 0f || vertical != 0f || a != 0f || b != 0){
                float decisionValue = Random.value;
                if(decisionValue < currentState.listenPercent){
                    if(horizontal > 0f){
                        currentGame.Right();
                    }
                    if(horizontal < 0f){
                        currentGame.Left();
                    }
                    if(vertical > 0f){
                        currentGame.Up();
                    }
                    if(vertical < 0f){
                        currentGame.Down();
                    }
                    if(a > 0f){
                        currentGame.A();
                    }
                    if(b > 0f){
                        currentGame.B();
                    }
                }
                else if(1 - decisionValue < currentState.wrongInputPercent){
                    RandomInput();
                }
            }
        }
    }


    public void RandomInput(){
        int randInput = Random.Range(0,6);
        switch(randInput){
            case 0:
                currentGame.A();
                break;
            case 1:
                currentGame.B();
                break;
            case 2:
                currentGame.Up();
                break;
            case 3:
                currentGame.Down();
                break;
            case 4:
                currentGame.Left();
                break;
            case 5:
                currentGame.Right();
                break;
        }
    }

    public void SetGame(ControllerInterface game){
        currentGame = game;
        startTime = Time.time;
        started = true;
    }

    public void StopGame(){
        started = false;
    }

    public void SetState(State state){
        currentState = state;
        GameObject gameChan = GameObject.FindGameObjectWithTag("ControllerChan");
        Image imageScript = gameChan.GetComponent<Image>();
    }



}
