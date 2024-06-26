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
    public Rigidbody2D enemyRb;
    public LayerMask cover;
    public AudioSource enemySource;
    public AudioClip runningClip;
    public AudioClip shootingClip;
    // Start is called before the first frame update
    void Start()
    {
        nodeCount = 0;
        originalFind = find;
        enemyRb.velocity = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        enemyRb.velocity = new Vector2(0f, 0f);
    }
    public void MoveNextTile()
    {
        if (currentPath == null) { return; }
        nodeCount++;
        currentPath.RemoveAt(0);
        if(find >= currentPath.Count) { find = currentPath.Count - 1; }
        bool foundPlayer = map.PlayerFound(currentPath[0 + find].x, currentPath[0 + find].y); //code to check where the player is
        Vector3 attackDist = transform.position - player.transform.position;
        attackDist.Normalize();
        float totalDist = Vector3.Distance(transform.position, player.transform.position);
        bool canAttack = true;
        for (float i = 0; i < totalDist; i += stepSize)
        {
            Vector3 tempPosition = transform.position - attackDist.normalized * i;
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
            foundPlayer = false;
            canAttack = false;
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
                if (!map.validEnemyMove(this.gameObject, currentPath[0].x, currentPath[0].y)) return;
                tileX = currentPath[0].x;
                tileY = currentPath[0].y;
                transform.position = map.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].y);
                currentPath = null;
                nodeCount = 0;
                find = originalFind;
                enemySource.clip = runningClip;
                enemySource.volume = 1f;
                enemySource.Play();
            }
        }
    }
    void Attack()
    {
        Debug.Log("Called");
        CheckIfCover();
        enemySource.clip = shootingClip;
        enemySource.volume = 0.2f;
        enemySource.Play();
        float chanceToHit = Random.Range(0f, 1f);
        if(chanceToHit <= threshold)
        {
            Debug.Log("Hit");
            PlayerHealth.TakeDamage();
            if(PlayerHealth.getHealth() <= 0)
            {
                Invoke("mapFunction", 1f);
            }
        }
        else
        {
            Debug.Log("Miss");
            return;
        }
    }
    void CheckIfCover()
    {
        Vector3 directionToEnemy = (transform.position - player.transform.position).normalized;
        float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
        if (Physics2D.Raycast(transform.position, directionToEnemy, distanceToEnemy, cover)) { Debug.Log("Found cover"); threshold = 1000f; }
        else threshold = 0.4f;
    }
    void mapFunction()
    {
        map.BeginPlayback();
    }
}
