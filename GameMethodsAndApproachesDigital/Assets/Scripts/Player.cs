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
    public GameObject moveField;
    public static bool moved = false;
    public float stepSize = 0.1f;
    public float hitChance = 0.6f;
    public int medkits = 1;
    public Rigidbody2D playerRb;
    public GameObject eventSpace;
    public UpdateStatus status;
    private MedKitController med;
    private EventSpace space;
    public float viewRadius = 1f;
    public LayerMask cover;
    public AudioSource playerSource;
    public AudioClip runningClip;
    public AudioClip shootingClip;
    public AudioClip trapAudio;
    bool wallFound;
    public Camera cam;
    private void Start()
    {
        wallFound = false;
        playerRb.velocity = new Vector2(0f, 0f);
        attackField.SetActive(false);
        med = GameObject.Find("HealthPickUp").GetComponent<MedKitController>();
        space = GameObject.Find("EventSpace").GetComponent<EventSpace>();
    }
    private void Update()
    {
        playerRb.velocity = new Vector2(0f, 0f);
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
        wallFound = false;
        playerSource.clip = runningClip;
        playerSource.volume = 1f;
        playerSource.Play();
        for (int i = 0; i < moves; i++)
        {
            if(direction == "right")
            {
                if (map.validMove(tileX + 1, tileY))
                {
                    tileX = tileX + 1;
                }
                else
                {
                    wallFound = true;
                    break; 
                }
            }
            else if (direction == "left")
            {
                if (map.validMove(tileX - 1, tileY))
                {
                    tileX = tileX - 1;
                }
                else
                {
                    wallFound = true;
                    break;
                }
            }
            else if (direction == "up")
            {
                if (map.validMove(tileX, tileY + 1))
                {
                    tileY = tileY + 1;
                }
                else
                {
                    wallFound = true;
                    break;
                }
            }
            else if (direction == "down")
            {
                if (map.validMove(tileX, tileY - 1))
                {
                    tileY = tileY - 1;
                }
                else
                {
                    wallFound = true;
                    break;
                }
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
        if (map.CheckForEnemy())
        {
            if(moved == true)
            {
                attackField.SetActive(false);
                moveField.SetActive(true);
            }
            else
            {
                attackField.SetActive(true);
                moveField.SetActive(false);
                yield break;
            }
        }
        else attackField.SetActive(false);
        EnemyTurn();
    }
    public void AttackEnemy(string attackString)
    {
        if (attackString == "attack")
        {
            map.CheckForEnemy();
            GameObject enemy = map.returnNearestEnemy();
            if (enemy == null) return;
            CheckIfCover(enemy);
            Vector3 attackDist = transform.position - enemy.transform.position;
            attackDist.Normalize();
            float totalDist = Vector3.Distance(transform.position, enemy.transform.position);
            for (float i = 0; i < totalDist; i += stepSize)
            {
                Vector3 tempPosition = transform.position - attackDist.normalized * i;
                Debug.Log("Current position" + tempPosition.x + " " + tempPosition.y);
                if (!map.isFloor((int)tempPosition.x, (int)tempPosition.y))
                {
                    status.CantAttack();
                    Invoke("StopRec", 1f);
                    return;
                }
            }
            playerSource.clip = shootingClip;
            playerSource.volume = 0.2f;
            playerSource.Play();
            float chanceToHit = Random.Range(0f, 1f);
            if (chanceToHit <= hitChance)
            {
                Debug.Log("Hit");
                map.RemoveEnemy(enemy);
                Destroy(enemy);
                attackField.SetActive(false);
            }
            else
            {
                StartCoroutine(MissedEnemy(0.15f, 0.4f));
                Debug.Log("Miss");
            }
            EnemyTurn();
        }
        else if (attackString == "pass")
        {
            Debug.Log("PASSED");
            EnemyTurn();
        }
        else
        {
            status.InvalidAttack();
            return;
        }
       
    }
    IEnumerator MissedEnemy(float duration, float magnitude)
    {
        Vector3 cameraPosition = cam.transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration) 
        {
            ReplaySystem.recordTurn = true;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cam.transform.localPosition = new Vector3(cameraPosition.x + x, cameraPosition.y + y, cameraPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.transform.localPosition = cameraPosition;
        Invoke("StopRec", 1f);
    }
    void CheckIfCover(GameObject e)
    {
        {
            
        }
        Vector3 directionToEnemy = (transform.position - e.transform.position).normalized;
        float distanceToEnemy = Vector3.Distance(transform.position, e.transform.position);
        if (Physics2D.Raycast(transform.position, directionToEnemy, distanceToEnemy, cover)) { Debug.Log("Found cover"); hitChance = 0f; }
        else hitChance = 0.6f;
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
            status.OverHealth();
            return;
        }
    }
    public void TriggerTrip()
    {
        if (EventSpace.triggerTrap == false)
        {
            EventSpace.triggerTrap = true;
            playerSource.clip = trapAudio;
            playerSource.Play();
            EnemyTurn();
        }
        else return;
    }
    void EnemyTurn()
    {
        map.setPlayerNode(tileX, tileY);
        map.MoveToPlayer(tileX, tileY);
        if (EventSpace.triggerTrap == true) Invoke("DestroyEvent", 1f);
        status.HealthOfPlayer();
        status.CurrentHealthKits(medkits);
        map.enemyStatus();
        if (med != null) med.MedKit();
        if (space != null) space.CheckForEnemies();
        moveField.SetActive(true);
        status.DisplayAllInfo();
        Invoke("StopRec", 1f);
    }
    void DestroyEvent()
    {
        Destroy(eventSpace);
    }
    void StopRec()
    {
        ReplaySystem.recordTurn = false;
    }
}
