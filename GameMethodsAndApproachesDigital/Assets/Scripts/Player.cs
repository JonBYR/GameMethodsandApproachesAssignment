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
            map.setPlayerNode(tileX, tileY);
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
    public IEnumerator moving(string direction, int moves)
    {
        for(int i = 0; i < moves; i++)
        {
            if(direction == "right")
            {
                if (map.validMove(tileX + 1, tileY))
                {
                    tileX = tileX + 1;
                }
                else break;
            }
            else if (direction == "left")
            {
                if (map.validMove(tileX - 1, tileY))
                {
                    tileX = tileX - 1;
                }
                else break;
            }
            else if (direction == "up")
            {
                if (map.validMove(tileX, tileY + 1))
                {
                    tileY = tileY + 1;
                }
                else break;
            }
            else if (direction == "down")
            {
                if (map.validMove(tileX, tileY - 1))
                {
                    tileY = tileY - 1;
                }
                else break;
            }
            transform.position = map.TileCoordToWorldCoord(tileX, tileY);
            yield return new WaitForSeconds(1f);
        }
        transform.position = map.TileCoordToWorldCoord(tileX, tileY);
        map.setPlayerNode(tileX, tileY);
        map.MoveToPlayer(tileX, tileY);
        //if (direction == "right")
        //{
        //    map.MoveTo(tileX+moves,tileY);
        //}
        //else if (direction == "left")
        //{
        //    map.MoveTo(tileX - moves, tileY);
        //}
        //else if (direction == "up")
        //{
        //    map.MoveTo(tileX, tileY + moves);
        //}
        //else if(direction == "down")
        //{
        //    map.MoveTo(tileX, tileY - moves);
        //}
    }
}
