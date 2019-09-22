using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnchance;
    public float cloudspawnchance;
    public GameObject jumpobstacle;
    public GameObject duckobstacle;
    public GameObject clouds;
    public GameObject player;
    public bool wait;
    bool pause;
    public List<GameObject> obs;
    public float starttime;
    public float quotient;
    public float spawnpercent;
    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        pause = player.GetComponent<Player>().pause;
        starttime = Time.time;
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
        spawnpercent = 100f - (Time.time - starttime) / quotient;
        float r = Random.Range(0f, spawnpercent);
        if(r < spawnchance)
        {
            if(!wait)
            {
                Spawn();
            }
        }

        float c = Random.Range(0f, 100f);
        if(c < cloudspawnchance)
        {
            if(!wait)
            {
                Cloud();   
            }
        }
    }

    void Spawn()
    {
        float r = Random.Range(0,2);
        if(r == 0)
        {
            obs.Add(Instantiate(jumpobstacle, new Vector3(player.transform.position.x + 20f, 1f, 0f), Quaternion.identity));
            StartCoroutine("startwait");
        }
        else if(r == 1)
        {
            obs.Add(Instantiate(duckobstacle, new Vector3(player.transform.position.x + 20f, 5.7f, 0f), Quaternion.identity));
            StartCoroutine("startwait");
        }      
        
    }

    void Cloud()
    {
        //float rr = Random.Range(4f, 8.1f);
        //Instantiate(clouds, new Vector3(player.transform.position.x + 20f, rr, 0f), Quaternion.identity);
    }

    IEnumerator startwait()
    {
        wait = true;
        yield return new WaitForSeconds(1f);
        wait = false;
    }


}
