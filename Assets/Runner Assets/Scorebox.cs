using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorebox : MonoBehaviour
{
    public GameObject player;
    bool pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pause = player.GetComponent<Player>().pause;
        if (!pause)
        {
            transform.position = new Vector3(player.transform.position.x - 1.23f, 0.5f, 0);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            player.GetComponent<Player>().score += 1;
        }
    }
}
