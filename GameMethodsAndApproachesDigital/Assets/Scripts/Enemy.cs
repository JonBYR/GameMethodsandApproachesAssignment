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
    private int nodeCount;
    public int find;
    private int originalFind;
    private GameObject player;
    public float threshold;
    // Start is called before the first frame update
    void Start()
    {
        nodeCount = 0;
        originalFind = find;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
    }
    public void MoveNextTile()
    {
        if (currentPath == null) { return; }
        nodeCount++;
        currentPath.RemoveAt(0);
        if(find >= currentPath.Count) { find = currentPath.Count - 1; }
        transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
        bool foundPlayer = map.PlayerFound(currentPath[0 + find].x, currentPath[0 + find].y);
        if (foundPlayer)
        {
            Attack();
            tileX = currentPath[0].x;
            tileY = currentPath[0].y;
            transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
            currentPath = null;
            nodeCount = 0;
            find = originalFind;
        }
        else
        {
            if (currentPath.Count == 2) //destination
            {
                tileX = currentPath[1].x;
                tileY = currentPath[1].y;
                //map.setCost(tileX, tileY, 1000000000);
                currentPath = null;
                nodeCount = 0;
                find = originalFind;
            }
            if (nodeCount >= 3)
            {
                tileX = currentPath[0].x;
                tileY = currentPath[0].y;
                transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
                currentPath = null;
                nodeCount = 0;
                find = originalFind;
            }
        }
    }
    void Attack()
    {
        Debug.Log("Called");
        float chanceToHit = Random.Range(0, 1);
        if(chanceToHit <= threshold)
        {
            Debug.Log("Hit");
            Destroy(player);
        }
        else
        {
            Debug.Log("Miss");
            return;
        }
    }
}
