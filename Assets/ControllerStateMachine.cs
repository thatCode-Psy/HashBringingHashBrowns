using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool timerStarted;
    float startTime;

    public float randomInputDelay = 5f;

    State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = DEFAULT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
