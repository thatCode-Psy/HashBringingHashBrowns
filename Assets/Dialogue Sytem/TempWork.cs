using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class TempWork : MonoBehaviour
{
    DialogueNode[] diaNode;
    string jsonFile; 
    // Start is called before the first frame update
    void Start()
    {
        //jsonFile = Directory.GetFiles(@"C:\Users\carlic2\Documents\Zaire's Shit\Unity Projects\HashBringingHashBrowns\Assets\Dialogue Sytem\Dialogue Data");
        jsonFile = File.ReadAllText(@"C:\Users\carlic2\Documents\Zaire's Shit\Unity Projects\HashBringingHashBrowns\Assets\Dialogue Sytem\Dialogue Data\MainFile.json");
        DialogueNode[] dia = JsonHelper.FromJson<DialogueNode>(jsonFile);
        Debug.Log(dia[2].id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
