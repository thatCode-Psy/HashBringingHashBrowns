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
    // Start is called before the first frame update


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(DialogueGraph g in allDialogue)
            {
                Debug.Log("Graph ID: " + g.id + " Node ID: " + g.mainGraph[0].nodeID);
            }
        }
    }
    void Start()
    {
        //jsonFiles = Directory.GetFiles(@"C:\Users\carlic2\Documents\Zaire's Shit\Unity Projects\HashBringingHashBrowns\Assets\Dialogue Sytem\Dialogue Data");
        TextAsset temp = Resources.Load<TextAsset>("DialogueData/MainFile");
        
        //foreach (string s in jsonFiles)
        //{
            //if(Path.GetExtension(s) == ".json"){
                dia = JsonHelper.FromJson<DialogueNode>(temp.text);
                
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
        List<Node> sNodes = FindSingles();
        int countId = 0; 
        foreach(Node h in hNodes)
        {
            DialogueGraph tempGraph = new DialogueGraph(h, countId);
            allDialogue.Add(tempGraph);
            countId++; 


        }

        foreach(Node s in sNodes)
        {
            DialogueGraph tempGraph = new DialogueGraph(s, countId);
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

    List<Node> FindSingles()
    {
        List<Node> singleNodes = new List<Node>();
        foreach (Node n in allNodes)
        {
            if (n.nodPos == Node.nodePosition.SINGLE)
            {
                singleNodes.Add(n);
            }
        }
        return singleNodes;
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
    public int Annoyed;
    public int Sad;
    public int Depressed;
    public int Excited; 
}
