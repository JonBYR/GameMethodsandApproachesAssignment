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
    public float stepSize = 0.5f;
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
        bool foundPlayer = map.PlayerFound(currentPath[0 + find].x, currentPath[0 + find].y);
        Vector3 attackDist = transform.position - player.transform.position;
        attackDist.Normalize();
        float totalDist = Vector3.Distance(transform.position, player.transform.position);
        bool canAttack = true;
        for (float i = 0; i < totalDist; i += stepSize)
        {
            Vector3 tempPosition = transform.position - attackDist.normalized * i;
            Debug.Log("Current position" + tempPosition.x + " " + tempPosition.y);
            if (!map.isFloor((int)tempPosition.x, (int)tempPosition.y))
            {
                canAttack = false;
                break;
            }
        }
        if (foundPlayer && canAttack)
        {
            Attack();
            //tileX = currentPath[0].x;
            //tileY = currentPath[0].y;
            //transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
            currentPath = null;
            nodeCount = 0;
            find = originalFind;
            return;
        }
        else
        {
            if (currentPath.Count == 1) //destination
            {
                //tileX = currentPath[1].x;
                //tileY = currentPath[1].y;
                //map.setCost(tileX, tileY, 1000000000);
                //transform.position = map.TileCoordToWorldCoord(currentPath[1].x, currentPath[1].y);
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
        float chanceToHit = Random.Range(0f, 1f);
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
