using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerResponse : MonoBehaviour
{
    [SerializeField]
    GameObject[] choices;
    public int currentChoice;
    public int numOfChoices;
    public Dialogue diaRef; 
    public GameEventListener choiceListener; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPlayerChoices()
    {
        numOfChoices = diaRef.currentNode.playerResponses.Count; 
        choiceListener.enabled = false;
        for(int i = 0; i < numOfChoices; i++)
        {
            choices[i].SetActive(true);
        }

    }

    public void SendPlayerChoice(int i)
    {
        currentChoice = i;
        DeactivateButtons(); 
    }

    public void ActivateButtons()
    {
        foreach (GameObject ga in choices)
        {
            ga.SetActive(true);
        }
    }

    public void DeactivateButtons()
    {
        choiceListener.enabled = true; 
        foreach (GameObject ga in choices)
        {
            ga.SetActive(false);
        }
    }

    public void SetChoiceText(int choiceNum, string line)
    {
        choices[choiceNum].GetComponentInChildren<TextMeshProUGUI>().text = line; 
    }
}
