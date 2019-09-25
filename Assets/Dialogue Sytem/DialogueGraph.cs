using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraph
{
    public List<Node> mainGraph = new List<Node>();
    public int id; 

    public DialogueGraph(Node node, int ID)
    {
        mainGraph.Add(node);
        id = ID; 
    }
    public void AddNode(Node nodeToAdd)
    {
        if (!IsInList(nodeToAdd))
        {
            if(mainGraph.Count == 0)
            {
                nodeToAdd.nodPos = Node.nodePosition.HEAD; 
            }
            else
            {
                nodeToAdd.nodPos = Node.nodePosition.BODY;
            }
            mainGraph.Add(nodeToAdd);
        }
    }

    public void AddEdge(Node parentNode, Node branchingNode)
    {
        int index = GetIndex(parentNode);
        if(index != -1)
        {
            mainGraph[index].addLink(branchingNode);
        }
    }

    public Node GetHead()
    {
        return mainGraph[0];
    }

    private bool IsInList(Node nodeFind)
    {
        foreach (Node n in mainGraph)
        {
            if (n == nodeFind)
            {
                return true;
            }
        }
        return false;
    }

    private int GetIndex(Node nodeToFind)
    {
        for(int i = 0; i < mainGraph.Count; i++)
        {
            if (mainGraph[i] == nodeToFind)
            {
                return i; 
            }
        }
        return -1; 
    }
}

public class Node
{
    public int nodeID;
    public List<Node> adjacentNodes = new List<Node>();
    public int[] adjNodeIDs; 
    public nodePosition nodPos; 
    lineType typeOfLine;
    public List<string> playerResponses = new List<string>(); 
    public string[] lines;
    public int Happy;
    public int Sad;
    public int Depressed;
    public int Annoyed; 
    public int Excited;
    public int Angry; 

    public Node(DialogueNode diaNode)
    {
        nodeID = diaNode.id;
        if(diaNode.Head && diaNode.Tail)
        {
            nodPos = nodePosition.SINGLE;
        }
        else if(diaNode.Head)
        {
            nodPos = nodePosition.HEAD;
        }
        else if (diaNode.Tail)
        {
            nodPos = nodePosition.TAIL; 
        }
        else
        {
            nodPos = nodePosition.BODY; 
        }
        foreach(string s in diaNode.playerResponses)
        {
            playerResponses.Add(s);
        }
        Happy = diaNode.Happy;
        Sad = diaNode.Sad;
        Depressed = diaNode.Depressed;
        Excited = diaNode.Excited;
        Annoyed = diaNode.Annoyed; 
        Angry = diaNode.Angry; 

        lines = diaNode.line;
        adjNodeIDs = diaNode.adjacentNodes;
    }
    //Links are based on player responses. If player has 3 responses, there should be 3 adjacent nodes. Map player choice to correspoding index number. 
    public void addLink(Node linkToAdd)
    {
        if (!InList(linkToAdd))
        {
            adjacentNodes.Add(linkToAdd);
        }
    }

    private bool InList(Node nodeFind)
    {
        foreach(Node n in adjacentNodes)
        {
            if(n == nodeFind)
            {
                return true; 
            }
        }
        return false; 
    }


    public enum lineType
    {
        PROMPT, RESPONSE
    }

    public enum nodePosition
    {
        HEAD, BODY, TAIL, SINGLE
    }
}


