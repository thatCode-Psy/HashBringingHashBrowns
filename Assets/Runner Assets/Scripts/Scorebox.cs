using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Scorebox : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI t;
    public TextMeshProUGUI h;
    bool pause;
    public int score;
    public int hiscore;
    public int fails;
    public TextAsset hiscorefile;
    string path = "Assets/HighScore.txt";
    public AudioClip ding;
    AudioSource asource;
    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
        asource.clip = ding;
       score = 0;
       hiscore = ReadString();
       h.text = "HI-SCORE: " + hiscore.ToString();
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
        asource.Play();
        t.text = "Score: " + score;
        if(score > hiscore)
        {
            hiscore = score;
            h.text = "Hi-score: " + hiscore;
            WriteString(hiscore.ToString());
        }
    }

    void WriteString(string hiscore)
    {
        StreamWriter writer = new StreamWriter(path,false);
        writer.WriteLine(hiscore);
        writer.Close();
    }

    int ReadString()
    {
        StreamReader reader = new StreamReader(path);
        int t = int.Parse(reader.ReadToEnd());
        reader.Close();
        return t;
    }

    public void Die()
    {
        score = 0;
        fails++;
        Debug.Log("Fails is now "+fails);
    }
}
