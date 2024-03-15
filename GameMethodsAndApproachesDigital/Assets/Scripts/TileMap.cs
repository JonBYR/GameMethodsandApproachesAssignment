using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    //tutorial: https://www.youtube.com/watch?v=2xXLhbQnHV4
    public GameObject player;
    public TileType[] tileTypes; //TileType will establish if a tile at positrion x,y is of a floor or a wall
    int[,] tiles; //2D array
    int mapSizeX = 10;
    int mapSizeY = 10;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0; //all tiles initialised to 0 (the floor)
            }
        }
        GenerateMap();
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
    public void MoveTo(int x, int y)
    {
        player.GetComponent<Player>().tileX = x;
        player.GetComponent<Player>().tileY = y; 
        player.transform.position = TileCoordToWorldCoord(x, y);
    }
}
