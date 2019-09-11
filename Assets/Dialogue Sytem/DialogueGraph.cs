using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraph : MonoBehaviour
{
    List<Node> mainGraph = new List<Node>(); 
    
    public void AddNode(Node nodeToAdd)
    {
        if (!IsInList(nodeToAdd))
        {
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
    int nodeID;
    List<Node> adjacentNodes;
    lineType typeOfLine;
    List<List<string>> linePool; 

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
}


