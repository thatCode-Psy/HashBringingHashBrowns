﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour , ControllerInterface
{
    Rigidbody2D rb;
    public float runspeed;
    public float acceleration;
    public float jumppower;
    public bool grounded;
    bool duckonland;
    public bool ducking;
    public int score;
    public bool pause;
    Vector3 pausepos;
    Vector3 pausevel;
    bool forceunduck;
    public bool unduckafterpause;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = true;
        score = 0;
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
           
        }
        else
        {
            runspeed += acceleration;
            
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(!grounded)
                {
                    duckonland = true;
                }
                else
                {
                    Duck();
                }
            }

            if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                duckonland = false;
                Unduck();
                if(pause)
                {
                    unduckafterpause = true;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        
    }

    void Pause()
    {
        pause = !pause;
    
        if(pause)
        {
            pausepos = transform.position;
            pausevel = rb.velocity;
            rb.velocity = Vector3.zero;
        }

        if(!pause)
        {
            rb.velocity = pausevel;
            if(unduckafterpause)
            {
                Unduck();
                unduckafterpause = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (pause)
        {
            rb.velocity = new Vector3(0, 0, 0);
            transform.position = pausepos;
        }
        else
        {
            rb.velocity = new Vector3(runspeed, rb.velocity.y, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Floor")
        {
            grounded = true;
            if(duckonland)
            {
                Duck();
                duckonland = false;
            }
        }

        if(collision.transform.tag == "Obstacle")
        {
            SceneManager.LoadScene("RunnerMinigame");
        }
    }

    public void A()
    {
        Jump();
    }

    public void B()
    {

    }

    public void Up()
    {

    }

    public void Down()
    {

    }

    public void Left()
    {

    }

    public void Right()
    {

    }

    public void Jump()
    {
        grounded = false;
        if(ducking)
        {
            forceunduck = true;
            Unduck();
            duckonland = true;
        }
        rb.AddForce(new Vector2(0,jumppower));
    }

    public void Duck()
    {
        if(pause)
        {
            unduckafterpause = false;
        }
        else
        {
            transform.localScale = new Vector3(3.33333333f, 1.5f,1);
            GetComponent<CircleCollider2D>().radius = 0.075f;
            transform.position = new Vector3(transform.position.x,.25f,0);
            ducking = true;
        }
        
    }

    public void Unduck()
    {
        if(pause)
        {
            unduckafterpause = true;
        }
        else if(grounded || forceunduck)
        {
            transform.position = new Vector3(transform.position.x, .5f, 0);
            transform.localScale = new Vector3(3.33333333f, 3.33333333f, 1);
            GetComponent<CircleCollider2D>().radius = 0.15f;
            ducking = false;
            forceunduck = false;
        }
        
    }
}