using System.Collections;
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
    public int score;
    public bool pause;
    Vector3 pausepos;
    Vector3 pausevel;

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
            
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                Jump();
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
        rb.AddForce(new Vector2(0,jumppower));
    }

}
