using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro; 

public class Dialogue : MonoBehaviour
{
    public TMP_Text textBox;
    public GameObject continueButton;
    public GameObject TextDisplay; 
    public PlayerResponse playerResponseRef; 
    public GameEventListener listenerRef;
    public DialogueGraph diaGraph;
    public DialogueLoader diaRef;
    private Node startNode;
    public Node currentNode;
    private bool dialougeInitiated = false; 
    public int dialogueId; 
    string[] goatText = new string[] { "1. Laik's super awesome custom typewriter script", "2. You can click to skip to the next text", "3.All text is stored in a single string array", "4. Ok, now we can continue", "5. End Kappa" };
    int currentlyDisplayingText = 0;


    public GameEvent choiceEvent;

    void Awake()
    {
    }

    private void Start()
    {


    }

    public void InitSetup(int graphID)
    {
        diaGraph = diaRef.GetDiaGraphByID(graphID);
        startNode = diaGraph.GetHead();
        currentNode = startNode;
        goatText = startNode.lines;
        TextDisplay.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(textBox.text != goatText[currentlyDisplayingText])
                GoToEnd(); 
        }
    }

    public void SkipToNextText()
    {
        if (currentlyDisplayingText < goatText.Length - 1)
        {
            StopAllCoroutines();
            currentlyDisplayingText++;
            StartCoroutine(AnimateText());
        }
        else if(currentNode.nodPos == Node.nodePosition.TAIL || currentNode.nodPos == Node.nodePosition.SINGLE)
        {
            /*
            textBox.text = "";
            continueButton.SetActive(false);
            */
            TextDisplay.SetActive(false);
            ControllerStateMachine.Instance.UnPauseGame();
        }
        else
        {
            textBox.text = "";
            continueButton.SetActive(false);
            choiceEvent.Raise();
        }
    }

    public void GoToEnd()
    {
        StopAllCoroutines(); 
        
        textBox.text = goatText[currentlyDisplayingText];
        continueButton.SetActive(true);
        dialougeInitiated = false;
    }


    IEnumerator AnimateText()
    {
        continueButton.SetActive(false);
        //Debug.Log("Length " + goatText.Length + " current sentence: " + currentlyDisplayingText);
        for (int i = 0; i < (goatText[currentlyDisplayingText].Length + 1); i++)
        {
            textBox.text = goatText[currentlyDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(.01f);
        }
        if (textBox.text == goatText[currentlyDisplayingText])
            continueButton.SetActive(true);
    }

    public void DialogueContinue()
    {
        currentlyDisplayingText = 0;

        int curnodeNum = 0;
       
            if (currentNode.adjacentNodes.Count == 3)
            {
                curnodeNum = playerResponseRef.currentChoice;
            }

            currentNode = currentNode.adjacentNodes[curnodeNum];
            goatText = currentNode.lines;


        ChangeGameChanState();

        int choiceNum = 0;
        foreach (string g in currentNode.playerResponses)
        {
            playerResponseRef.SetChoiceText(choiceNum, g);
            choiceNum++;
        }
        StartCoroutine(AnimateText());
        listenerRef.enabled = false;
    }

    public void DialogueInit(int graphID)
    {
        InitSetup(graphID);
        Debug.Log("Graph Id is " + graphID);
        currentlyDisplayingText = 0; 
        /*
        if (dialougeInitiated)
        {
            int curnodeNum = 0;
            if (currentNode.adjacentNodes.Count == 3)
            {
                Debug.Log("Dialogue Started");

                curnodeNum = playerResponseRef.currentChoice;
            }
            currentNode = currentNode.adjacentNodes[curnodeNum];  
            goatText = currentNode.lines;
        }
        else
        {
            dialougeInitiated = true; 
        }
        */
        ChangeGameChanState(); 

        int choiceNum = 0; 
        foreach(string g in currentNode.playerResponses)
        {
            playerResponseRef.SetChoiceText(choiceNum, g);
            choiceNum++; 
        }
        StartCoroutine(AnimateText());
        listenerRef.enabled = false; 



    }


    void ChangeGameChanState()
    {
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.HAPPY, currentNode.Happy);
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.SAD, currentNode.Sad);
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.DEPRESSED, currentNode.Depressed);
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.EXCITED, currentNode.Excited);
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.ANGRY, currentNode.Annoyed);
        ControllerStateMachine.Instance.SetState(ControllerStateMachine.DEFAULT, currentNode.Default);



    }

}

