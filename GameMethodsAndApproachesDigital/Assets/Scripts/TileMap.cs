using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour
{
    
    //tutorial: https://www.youtube.com/watch?v=2xXLhbQnHV4
    public GameObject player; //should actually be any unit rather than player
    public TileType[] tileTypes; //TileType will establish if a tile at positrion x,y is of a floor or a wall
    int[,] tiles; //2D array
    Node[,] graph;
    List<Node> currentPath = null;
    int mapSizeX = 10;
    int mapSizeY = 10;
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Player>().tileX = (int)player.transform.position.x;
        player.GetComponent<Player>().tileY = (int)player.transform.position.y;
        player.GetComponent<Player>().map = this;
        tiles = new int[mapSizeX, mapSizeY];
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0; //all tiles initialised to 0 (the floor)
            }
        }
        GenerateMap();
        GenerateGraph();
        GenerateVisuals();
    }
    float CostOfTile(int x, int y)
    {
        TileType t = tileTypes[tiles[x, y]];
        return t.movementCost; //walls would have a huge cost to ensure it is undesirable to enter
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void GenerateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }
        
    }
    void GenerateGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++) //Initialise graph and initilise each node
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++) //find neighbours
            {
                if(x > 0) graph[x, y].neighbours.Add(graph[x - 1, y]);
                if(x < mapSizeX-1) graph[x, y].neighbours.Add(graph[x + 1, y]);
                if(y > 0) graph[x, y].neighbours.Add(graph[x, y - 1]);
                if(y < mapSizeY-1) graph[x, y].neighbours.Add(graph[x, y + 1]);
            }
        }
    }
    void GenerateVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tile = tileTypes[tiles[x, y]];
                GameObject g = (GameObject)Instantiate(tile.visualPrefab, new Vector3(x, y, 0), Quaternion.identity);
                ClickableTile ct = g.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }
        }
    }
    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }
    public void MoveTo(int x, int y) //Dijsktra's algorithm
    {
        player.GetComponent<Player>().currentPath = null;
        Node source = graph[player.GetComponent<Player>().tileX, player.GetComponent<Player>().tileY]; //starting position for player
        Node target = graph[x, y];
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        dist[source] = 0; //source node has no distance as it defined
        prev[source] = null; //not moved
        List<Node> unvisted = new List<Node>();
        foreach(Node n in graph) //initialise all nodes to have infinite distance
        {
            if(n != source)
            {
                dist[n] = Mathf.Infinity;
                prev[n] = null;
            }
            unvisted.Add(n); //add all nodes that have not been vistied yet
        }
        while(unvisted.Count > 0)
        {
            Node u = null;
            foreach(Node possible in unvisted) //u will be the unvisted node with the smallest distance as it is not infinity
            {
                if(u == null || dist[possible] < dist[u])
                {
                    u = possible;
                }
            }
            if (u == target) break; //if current node is target node we no longer are calculating the path
            unvisted.Remove(u);
            foreach(Node v in u.neighbours) //looks through the neighbours of the vertex
            {
                float temp = dist[u] + CostOfTile(v.x, v.y);
                if(temp < dist[v])
                {
                    dist[v] = temp;
                    prev[v] = u;
                }

            }
        }
        if (prev[target] == null) //no route
        {
            return;
        }
        currentPath = new List<Node>();
        Node curr = target;
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr]; //gets the previous Node from the path
        }
        currentPath.Reverse(); //unit needs to start from
        player.GetComponent<Player>().currentPath = currentPath;
        player.GetComponent<Player>().canMove = true;
        InvokeRepeating("MoveUnit", 0f, 1f);
    }
    void MoveUnit()
    {
        player.GetComponent<Player>().MoveNextTile();
        if (player.GetComponent<Player>().currentPath == null)
        {
            CancelInvoke("MoveUnit");
        }
    }
}
