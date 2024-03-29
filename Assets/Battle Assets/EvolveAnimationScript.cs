﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolveAnimationScript : MonoBehaviour {
    public Fighter player;
    public Image evolvingSprite;
    public float evolveSpeed = 1f;

    private bool evolving = false;
    private bool goingDown = true;

    private float evolveTimer = 5f;
    private float evolveTimerStart = 0f;

    private void OnEnable() {
        evolving = true;
        evolveTimerStart = evolveTimer;
    }

    // Update is called once per frame
    void Update() {
        if(evolving) {
            Color newColor = evolvingSprite.color;
            evolveTimer -= Time.deltaTime;

            if(goingDown) {
                newColor.r -= evolveSpeed * Time.deltaTime;
                newColor.g -= evolveSpeed * Time.deltaTime;
                newColor.b -= evolveSpeed * Time.deltaTime;

                evolvingSprite.color = newColor;

                if(evolveTimer <= 0f && newColor.r <= 0f) {
                    evolving = false;
                    evolveTimer = evolveTimerStart;
                } else if (newColor.r <= 0f) {
                    goingDown = false;
                }
            } else {
                newColor.r += evolveSpeed * Time.deltaTime;
                newColor.g += evolveSpeed * Time.deltaTime;
                newColor.b += evolveSpeed * Time.deltaTime;

                evolvingSprite.color = newColor;

                if(newColor.r >= .75f) {
                    goingDown = true;
                }
            }
        } else {
            player.StopEvolve();
        }
    }
}
