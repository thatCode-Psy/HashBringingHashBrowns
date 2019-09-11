using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnchance;
    public GameObject jumpobstacle;
    public GameObject duckobstacle;
    public GameObject player;
    public bool wait;
    bool pause;
    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        pause = player.GetComponent<Player>().pause;
    }

    // Update is called once per frame
    void Update()
    {
        pause = player.GetComponent<Player>().pause;
        if (!pause)
        {
            SpawnCheck();
        }
        
    }

    void SpawnCheck()
    {
        float r = Random.Range(0f, 100f);
        if(r < spawnchance)
        {
            if(!wait)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        float r = Random.Range(0,2);
        if(r == 0)
        {
            Instantiate(jumpobstacle, new Vector3(player.transform.position.x + 20f, 1f, 0f), Quaternion.identity);
            StartCoroutine("startwait");
        }
        else
        {
            Instantiate(duckobstacle, new Vector3(player.transform.position.x + 20f, 5.7f, 0f), Quaternion.identity);
            StartCoroutine("startwait");
        }
        
    }

    IEnumerator startwait()
    {
        wait = true;
        yield return new WaitForSeconds(1);
        wait = false;
    }


}
