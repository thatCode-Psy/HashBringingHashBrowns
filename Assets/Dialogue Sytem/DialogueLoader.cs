using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    List<DialogueGraph> allDialogue = new List<DialogueGraph>();
    List<DialogueNode> diaNodes = new List<DialogueNode>();
    public Dialogue diaRef; 
    DialogueNode[] dia; 
    List<Node> allNodes = new List<Node>(); 
    string jsonFiles; 
    // Start is called before the first frame update
    void Start()
    {
        //jsonFiles = Directory.GetFiles(@"C:\Users\carlic2\Documents\Zaire's Shit\Unity Projects\HashBringingHashBrowns\Assets\Dialogue Sytem\Dialogue Data");
        jsonFiles = File.ReadAllText(@"C:\Users\carlic2\Documents\Zaire's Shit\Unity Projects\HashBringingHashBrowns\Assets\Dialogue Sytem\Dialogue Data\MainFile.json");
        //foreach (string s in jsonFiles)
        //{
            //if(Path.GetExtension(s) == ".json"){
                dia = JsonHelper.FromJson<DialogueNode>(jsonFiles);
                
            //}
       // }
        foreach (DialogueNode d in dia)
        {
            Node tempNode = new Node(d);
            
            allNodes.Add(tempNode);
        }
        
        foreach(Node t in allNodes)
        {
            foreach(int d in t.adjNodeIDs)
            {
                Node tempNode = FindByID(d);
                if(tempNode != null)
                {
                    t.addLink(tempNode);
                }
            }
        }

        List<Node> hNodes = FindHeads();

        int countId = 0; 
        foreach(Node h in hNodes)
        {
            DialogueGraph tempGraph = new DialogueGraph(h, countId);
            allDialogue.Add(tempGraph);
            countId++; 


        }

    }


    public DialogueGraph GetDiaGraphByID(int id)
    {
        foreach(DialogueGraph s in allDialogue)
        {
            if(id == s.id)
            {
                return s; 
            }
        }
        return null; 
    }


    Node FindByID(int id)
    {
        foreach(Node s in allNodes)
        {
            if(s.nodeID == id)
            {
                return s;
            }
        }
        return null; 
    }



    List<Node> FindHeads()
    {
        List<Node> headNodes = new List<Node>();  
        foreach(Node n in allNodes)
        {
            if(n.nodPos == Node.nodePosition.HEAD)
            {
                headNodes.Add(n);
            }
        }
        return headNodes; 
    }
    
}

[System.Serializable]
public class DialogueNode
{
    public int id;
    public string[] line;
    public string[] playerResponses;
    public int[] adjacentNodes;
    public bool Head;
    public bool Tail;
    public int Happy;
    public int Angry;
    public int Sad;
    public int Depressed;
    public int Excited; 
}
