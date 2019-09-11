﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro; 

public class Dialogue : MonoBehaviour
{
    public TMP_Text textBox;
    public GameObject continueButton;
    private bool eventRecieved = false; 
    //Store all your text in this string array
    string[] goatText = new string[] { "1. Laik's super awesome custom typewriter script", "2. You can click to skip to the next text", "3.All text is stored in a single string array", "4. Ok, now we can continue", "5. End Kappa" };
    int currentlyDisplayingText = 0;


    void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(textBox.text != goatText[currentlyDisplayingText])
                GoToEnd(); 
        }
    }
    //This is a function for a button you press to skip to the next text
    public void SkipToNextText()
    {
        StopAllCoroutines();
        currentlyDisplayingText++;
        //If we've reached the end of the array, do anything you want. I just restart the example text
        if (currentlyDisplayingText > goatText.Length - 1)
        {
            currentlyDisplayingText = 0;
        }
        StartCoroutine(AnimateText());
    }

    public void GoToEnd()
    {
        StopAllCoroutines(); 
        
        textBox.text = goatText[currentlyDisplayingText];
        continueButton.SetActive(true);
    }


    IEnumerator AnimateText()
    {
        continueButton.SetActive(false);
        for (int i = 0; i < (goatText[currentlyDisplayingText].Length + 1); i++)
        {
            textBox.text = goatText[currentlyDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(.01f);
        }
        if (textBox.text == goatText[currentlyDisplayingText])
            continueButton.SetActive(true);
    }


    public void DialogueInit()
    {
        if (eventRecieved == false)
        {
            StartCoroutine(AnimateText());
            eventRecieved = true; 
        }
    }
}
