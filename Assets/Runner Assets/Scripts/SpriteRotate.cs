using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotate : MonoBehaviour
{
    public float rotatespeed;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.pause)
        {
        transform.Rotate(new Vector3(0f,0f,rotatespeed),Space.Self);
        }
    }
}
