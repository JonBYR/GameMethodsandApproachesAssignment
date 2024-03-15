using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public List<Node> currentPath = null;
    public TileMap map;
    public bool canMove = false;
    bool enemyFound = false;
    private void Start()
    {
        StartCoroutine("MoveNextTile");
    }
    public void MoveNextTile()
    {
        if(currentPath == null) { canMove = false; return; }
        if (enemyFound)
        {
            tileX = currentPath[0].x;
            tileY = currentPath[0].y;
            currentPath = null;
            canMove = false;
        }
        currentPath.RemoveAt(0);
        transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
        
        if (currentPath.Count == 1) //destination
        {
            tileX = currentPath[0].x;
            tileY = currentPath[0].y;
            currentPath = null;
            canMove = false;
            map.MoveToPlayer(tileX, tileY);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            enemyFound = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemyFound = false;
        }
    }
}
