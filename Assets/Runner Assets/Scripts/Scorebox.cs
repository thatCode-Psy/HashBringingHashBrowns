using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scorebox : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI t;
    bool pause;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        pause = player.GetComponent<Player>().pause;
        if (!pause)
        {
            transform.position = new Vector3(player.transform.position.x - 1.23f, 2.6f, 0);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            player.GetComponent<Player>().score += 1;
            score = player.GetComponent<Player>().score;
            IncreaseScore();
        }
    }

    void IncreaseScore()
    {
        t.text = "Score: " + score;
    }
}
