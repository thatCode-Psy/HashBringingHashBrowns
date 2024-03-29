﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public Spawner spawner;
    public TextMeshProUGUI scoretext;
    public AudioClip jump;
    public AudioClip duck;
    public AudioClip fail;
    AudioSource asource;
    public Scorebox sb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = true;
        score = 0;
        pause = false;
        ControllerStateMachine.Instance.SetGame(this);
        asource = GetComponent<AudioSource>();
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
            
            // if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
            // {
            //     Jump();
            // }

            // if (Input.GetKeyDown(KeyCode.DownArrow))
            // {
            //     if(!grounded)
            //     {
            //         duckonland = true;
            //     }
            //     else
            //     {
            //         Duck();
            //     }
            // }
            /*
            if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.B))
            {
                duckonland = false;
                Unduck();
                if(pause)
                {
                    unduckafterpause = true;
                }
            }*/
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        
    }

    public void A()
    {
        
        if(grounded){
            Jump();
        }
    }

    public void B()
    {
        if (!pause)
        {
            if (sb.hiscore < 100)
            {
                Pause();
                ControllerStateMachine.Instance.StopPauseTimer();
                Dialogue dialogue = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>();
                dialogue.DialogueInit(2);
            }
            else
            {
                ControllerStateMachine.Instance.StopGame();
                SceneManager.LoadScene("GameSelect");
            }
        }
    }

    public void Up()
    {
        if(grounded){
            Jump();
        }
        
    }

    public void Down()
    {
        if(!grounded)
        {
            //duckonland = true;
        }
        else if(!ducking)
        {
            Duck();
        }
        else if(ducking)
        {
            Unduck();
        }
    }

    public void Left()
    {

    }

    public void Right()
    {

    }

    public void Pause()
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
            Die();
        }
    }

    void Die()
    {
        foreach(GameObject g in spawner.obs)
        {
            Destroy(g);
        }

        spawner.obs.Clear();

        score = 0;
        scoretext.text = "Score: 0";
        runspeed = 5;
        asource.clip = fail;
        asource.Play();
        sb.Die();
    }


    public void Jump()
    {
        grounded = false;
        if(ducking)
        {
            forceunduck = true;
            Unduck();
            //duckonland = true;
        }
        rb.AddForce(new Vector2(0,jumppower));
        asource.clip = jump;
        asource.Play();
    }

    public void Duck()
    {
        if(pause)
        {

        }
        else
        {
            transform.localScale = new Vector3(3.33333333f, 1.5f,1);
            GetComponent<CircleCollider2D>().radius = 0.075f;
            transform.position = new Vector3(transform.position.x,.25f,0);
            ducking = true;
            asource.clip = duck;
            asource.Play();
        }
        
    }

    public void Unduck()
    {
        if(pause)
        {
            
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
    public List<int> GetPossibleDialogueNodes(){
        List<int> ret = new List<int>();
        if(sb.fails > 9 && sb.score < 100)
        {
            ret.Add(0);
            Debug.Log("Adding 1");
            ret.Add(0);
        }
        if(sb.fails < 10 && sb.score > 49)
        {
            ret.Add(1);
            Debug.Log("Adding 11");
            ret.Add(1);
        }
        if(sb.score > 99)
        {
            ret.Add(3);
        }
        ret.Add(10);
        ret.Add(11);
        ret.Add(12);





        return ret;
    }

}
