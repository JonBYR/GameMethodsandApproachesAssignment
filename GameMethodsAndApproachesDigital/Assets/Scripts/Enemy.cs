using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public List<Node> currentPath = null;
    public TileMap map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveNextTile()
    {
        if (currentPath == null) { return; }
        currentPath.RemoveAt(0);
        transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);

        if (currentPath.Count == 2) //destination
        {
            tileX = currentPath[1].x;
            tileY = currentPath[1].y;
            map.setCost(tileX, tileY, 1000000000);
            currentPath = null;
        }
    }
}
