using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour
{
    
    //tutorial: https://www.youtube.com/watch?v=2xXLhbQnHV4
    public GameObject player; //should actually be any unit rather than player
    public List<GameObject> enemies;
    public TileType[] tileTypes; //TileType will establish if a tile at positrion x,y is of a floor or a wall
    Node playerNode;
    public float playerMovement;
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
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().tileX = (int)enemy.transform.position.x;
            enemy.GetComponent<Enemy>().tileY = (int)enemy.transform.position.y;
            enemy.GetComponent<Enemy>().map = this;
        }
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
    public void setCost(int x, int y, int value)
    {
        TileType t = tileTypes[tiles[x, y]];
        t.movementCost = value;
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
        tiles[1, 2] = 1;
        tiles[1, 3] = 1;
        tiles[1, 4] = 1;
        tiles[1, 5] = 1;
        tiles[1, 6] = 1;
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
    public void setPlayerNode(int x, int y)
    {
        playerNode = graph[x, y];
    }
    public Node getPlayerNode()
    {
        return playerNode;
    }
    public bool PlayerFound(int x, int y)
    {
        if(x == playerNode.x && y == playerNode.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MoveTo(int x, int y) //Dijsktra's algorithm
    {
        TileType tile = tileTypes[tiles[x, y]];
        if (tile.isClickable == false) return;
        Transform playerTransform = player.GetComponent<Transform>();
        if (Vector3.Distance(playerTransform.position, TileCoordToWorldCoord(x, y)) >= playerMovement) return;
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
    public void MoveToPlayer(int x, int y)
    {
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().currentPath = null;
            Node source = graph[enemy.GetComponent<Enemy>().tileX, enemy.GetComponent<Enemy>().tileY]; //starting position for player
            setCost(enemy.GetComponent<Enemy>().tileX, enemy.GetComponent<Enemy>().tileY, 0);
            Node target = graph[x, y];
            Dictionary<Node, float> dist = new Dictionary<Node, float>();
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
            dist[source] = 0; //source node has no distance as it defined
            prev[source] = null; //not moved
            List<Node> unvisted = new List<Node>();
            foreach (Node n in graph) //initialise all nodes to have infinite distance
            {
                if (n != source)
                {
                    dist[n] = Mathf.Infinity;
                    prev[n] = null;
                }
                unvisted.Add(n); //add all nodes that have not been vistied yet
            }
            while (unvisted.Count > 0)
            {
                Node u = null;
                foreach (Node possible in unvisted) //u will be the unvisted node with the smallest distance as it is not infinity
                {
                    if (u == null || dist[possible] < dist[u])
                    {
                        u = possible;
                    }
                }
                if (u == target) break; //if current node is target node we no longer are calculating the path
                unvisted.Remove(u);
                foreach (Node v in u.neighbours) //looks through the neighbours of the vertex
                {
                    float temp = dist[u] + CostOfTile(v.x, v.y);
                    if (temp < dist[v])
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
            enemy.GetComponent<Enemy>().currentPath = currentPath;
        }
        foreach(GameObject enemy in enemies)
        {
            while(enemy.GetComponent<Enemy>().currentPath != null) 
            {
                enemy.GetComponent<Enemy>().MoveNextTile();
            }
        }
    }
    void MoveUnit()
    {
        player.GetComponent<Player>().MoveNextTile();
        if (player.GetComponent<Player>().currentPath == null)
        {
            CancelInvoke("MoveUnit");
        }
    }
    void MoveEnemy(GameObject enemy)
    {

    }
}
