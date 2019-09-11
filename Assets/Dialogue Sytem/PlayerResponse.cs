using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResponse : MonoBehaviour
{
    [SerializeField]
    GameObject[] choices;
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
        choiceListener.enabled = false;
        foreach(GameObject ga in choices)
        {
            ga.SetActive(true);
        }
    }

    public void SendPlayerChoice(string s)
    {
        Debug.Log(s);
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
}
