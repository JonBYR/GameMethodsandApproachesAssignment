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
    public bool canAttackEnemy = false;
    public GameObject attackField;
    public static bool moved = false;
    public float stepSize = 0.1f;
    public float hitChance = 0.6f;
    public int medkits = 1;
    public GameObject eventSpace;
    private void Start()
    {
        StartCoroutine("MoveNextTile");
        attackField.SetActive(false);
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
        moved = false;
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
        if (moves > 2) 
        {
            moved = true;
            attackField.SetActive(false);
        }
        transform.position = map.TileCoordToWorldCoord(tileX, tileY);
        if (map.CheckForEnemy()) attackField.SetActive(true);
        else attackField.SetActive(false);
        EnemyTurn();
    }
    public void AttackEnemy(string attackString)
    {
        if(attackString != "pass")
        {
            GameObject enemy = map.returnNearestEnemy();
            Vector3 attackDist = transform.position - enemy.transform.position;
            attackDist.Normalize();
            float totalDist = Vector3.Distance(transform.position, enemy.transform.position);
            for (float i = 0; i < totalDist; i += stepSize)
            {
                Vector3 tempPosition = transform.position - attackDist.normalized * i;
                Debug.Log("Current position" + tempPosition.x + " " + tempPosition.y);
                if (!map.isFloor((int)tempPosition.x, (int)tempPosition.y))
                {
                    break;
                }
            }
            float chanceToHit = Random.Range(0f, 1f);
            if (chanceToHit <= hitChance)
            {
                Debug.Log("Hit");
                map.RemoveEnemy(enemy);
                Destroy(enemy);
            }
            else
            {
                Debug.Log("Miss");
            }
            EnemyTurn();
        }
    }
    public void Heal()
    {
        Debug.Log("Call heal");
        if(medkits > 0 && PlayerHealth.getHealth() < 3)
        {
            Debug.Log("Healing");
            PlayerHealth.setHealth(3);
            medkits = 0;
            EnemyTurn();
        }
        else
        {
            return;
        }
    }
    public void TriggerTrip()
    {
        if (EventSpace.triggerTrap == false)
        {
            EventSpace.triggerTrap = true;
            EnemyTurn();
        }
        else return;
    }
    void EnemyTurn()
    {
        map.setPlayerNode(tileX, tileY);
        map.MoveToPlayer(tileX, tileY);
        if (EventSpace.triggerTrap == true) Invoke("DestroyEvent", 1f);
    }
    void DestroyEvent()
    {
        Destroy(eventSpace);
    }
}
