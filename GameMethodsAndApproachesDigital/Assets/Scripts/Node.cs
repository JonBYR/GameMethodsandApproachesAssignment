using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> neighbours = new List<Node>(); //list of nodes next to the nodes
    public int x;
    public int y;
    public Node()
    {
        neighbours = new List<Node>();
    }
    public float DistanceTo(Node n)
    {
        return Vector2.Distance(new Vector2(x, y), new Vector2(n.x, n.y)); //gets distance between current node and a neighbouring node
    }
}
