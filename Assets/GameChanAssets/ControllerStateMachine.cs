using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class State{
    public float listenPercent;
    public float randomInputPercent;
    public float wrongInputPercent;
    public State(float listenPercent, float randomInputPercent, float wrongInputPercent){
        this.listenPercent = listenPercent;
        this.randomInputPercent = randomInputPercent;
        this.wrongInputPercent = wrongInputPercent;
    }

    public bool Equals(State other){
        return listenPercent == other.listenPercent && randomInputPercent == other.randomInputPercent && wrongInputPercent == other.wrongInputPercent;
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

    public Sprite DefaultImage;
    public Sprite HappyImage;
    public Sprite ExcitedImage;
    public Sprite AngryImage;
    public Sprite SadImage;
    public Sprite DepressedImage;

    float startTime;
    bool started;



    public float randomInputDelay = 5f;
    public float pauseInterval = 30f;
    float pauseStartTime;


    State currentState;


    int[] stateValues;

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
        stateValues = new int[6];
        stateValues[0] = 1;
        pauseStartTime = -1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started && pauseStartTime != -1f){
            if(Time.time - startTime > randomInputDelay){
                startTime = Time.time;
                if(currentState.randomInputPercent > Random.value){
                    RandomInput();
                }
            }
            
            bool left = Input.GetButtonDown("Left");
            bool right = Input.GetButtonDown("Right");
            bool up = Input.GetButtonDown("Up");
            bool down = Input.GetButtonDown("Down");
            bool a = Input.GetButtonDown("A");
            bool b = Input.GetButtonDown("B");
            bool escape = Input.GetButton("Escape");
            if(left || right || up || down || a || b || escape){
                float decisionValue = Random.value;
                if(decisionValue < currentState.listenPercent){
                    if(right){
                        currentGame.Right();
                    }
                    if(left){
                        currentGame.Left();
                    }
                    if(up){
                        currentGame.Up();
                    }
                    if(down){
                        currentGame.Down();
                    }
                    if(a){
                        currentGame.A();
                    }
                    if(b){
                        currentGame.B();
                    }
                    if(escape){
                        ExitGame();
                    }
                }
                else if(1 - decisionValue < currentState.wrongInputPercent){
                    RandomInput();
                }
                else{
                    print("not listening");
                }
            }
            if(Time.time - pauseStartTime >= pauseInterval && pauseStartTime != -1f){
                print("pause");
                pauseStartTime = -1f;
                currentGame.Pause();
                Dialogue dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();
                dialogue.InitSetup(0);
            }
        }
    }


    public void RandomInput(){
        int randInput = Random.Range(0, 7);
        print("doing random input");
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
            case 6:
                ExitGame();
                break;
        }
    }

    public void SetGame(ControllerInterface game){
        currentGame = game;
        startTime = Time.time;
        pauseStartTime = Time.time;
        started = true;
    }

    public void StopGame(){
        started = false;
    }

    public void ExitGame(){
        SceneManager.LoadScene("GameSelect");
    }

    //For non-fuzzy logic
    // public void SetState(State state){
    //     currentState = state;
    //     GameObject gameChan = GameObject.FindGameObjectWithTag("ControllerChan");
    //     Image imageScript = gameChan.GetComponent<Image>();
    // }

    //fuzzy logic
    public void SetState(State state, int valueChange){
        if(state.Equals(DEFAULT)){
            stateValues[0] += valueChange;
        }
        else if(state.Equals(HAPPY)){
            stateValues[1] += valueChange;
        }
        else if(state.Equals(EXCITED)){
            stateValues[2] += valueChange;
        }
        else if(state.Equals(ANGRY)){
            stateValues[3] += valueChange;
        }
        else if(state.Equals(SAD)){
            stateValues[4] += valueChange;
        }
        else if(state.Equals(DEPRESSED)){
            stateValues[5] += valueChange;
        }
        for(int i = 0; i < stateValues.Length; ++i){
            if(stateValues[i] < 0){
                stateValues[i] = 0;
            }
        }
        SetImage();
        AdjustState();
    } 
    
    private void SetImage(){
        GameObject gameChan = GameObject.FindGameObjectWithTag("ControllerChan");
        Image imageScript = gameChan.GetComponent<Image>();
        if(Mathf.Max(stateValues) == stateValues[0]){
            imageScript.overrideSprite = DefaultImage;
        }
        else if(Mathf.Max(stateValues) == stateValues[1]){
            imageScript.overrideSprite = HappyImage;
        }
        else if(Mathf.Max(stateValues) == stateValues[2]){
            imageScript.overrideSprite = ExcitedImage;
        }
        else if(Mathf.Max(stateValues) == stateValues[3]){
            imageScript.overrideSprite = AngryImage;
        }
        else if(Mathf.Max(stateValues) == stateValues[4]){
            imageScript.overrideSprite = SadImage;
        }
        else if(Mathf.Max(stateValues) == stateValues[5]){
            imageScript.overrideSprite = DepressedImage;
        }
        
    }

    private void AdjustState(){
        float listenPercent = 0f;
        float randomInputPercent = 0f;
        float wrongInputPercent = 0f;
        float totalValue = 0f;
        for(int i = 0; i < stateValues.Length; ++i){
            totalValue += stateValues[i];
        }
        listenPercent += stateValues[0] * DEFAULT.listenPercent;
        randomInputPercent += stateValues[0] * DEFAULT.randomInputPercent;
        wrongInputPercent += stateValues[0] * DEFAULT.wrongInputPercent;
        listenPercent += stateValues[1] * HAPPY.listenPercent;
        randomInputPercent += stateValues[1] * HAPPY.randomInputPercent;
        wrongInputPercent += stateValues[1] * HAPPY.wrongInputPercent;
        listenPercent += stateValues[2] * EXCITED.listenPercent;
        randomInputPercent += stateValues[2] * EXCITED.randomInputPercent;
        wrongInputPercent += stateValues[2] * EXCITED.wrongInputPercent;
        listenPercent += stateValues[3] * ANGRY.listenPercent;
        randomInputPercent += stateValues[3] * ANGRY.randomInputPercent;
        wrongInputPercent += stateValues[3] * ANGRY.wrongInputPercent;
        listenPercent += stateValues[4] * SAD.listenPercent;
        randomInputPercent += stateValues[4] * SAD.randomInputPercent;
        wrongInputPercent += stateValues[4] * SAD.wrongInputPercent;
        listenPercent += stateValues[5] * DEPRESSED.listenPercent;
        randomInputPercent += stateValues[5] * DEPRESSED.randomInputPercent;
        wrongInputPercent += stateValues[5] * DEPRESSED.wrongInputPercent;
        listenPercent /= totalValue;
        randomInputPercent /= totalValue;
        wrongInputPercent /= totalValue;
        currentState = new State(listenPercent, randomInputPercent, wrongInputPercent);
    }

    public void UnPauseGame(){
        Debug.Log("UNPAUSED");
        currentGame.Pause();
        pauseStartTime = Time.time;
    }
}
