using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed;
    public bool pos;
    int counter;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if(counter >= speed)
        {
            Swap();
            counter = 0;
        }
        transform.position = new Vector3(player.transform.position.x+3.8f, transform.position.y, 0); 
    }

    void Swap()
    {
        if(pos)
        {
            transform.position = new Vector3(transform.position.x,4.85f,0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x,5.68f,0);
        }
        pos = !pos;
    }
}
