using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpace : MonoBehaviour
{
    public static bool triggerTrap = false;
    public static bool trapTriggered = false;
    public LayerMask layer;
    public UpdateStatus status;
    public TileMap map;
    public Vector2 colliderSize;
    // Start is called before the first frame update
    void Start()
    {
        triggerTrap = false;
        trapTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerTrap == true)
        {
            Collider2D[] allAttackables = Physics2D.OverlapBoxAll(transform.localPosition, colliderSize, 0f, layer);
            if(allAttackables != null)
            {
                foreach(Collider2D e in allAttackables)
                {
                    if (e.gameObject.tag == "Enemy")
                    {
                        map.RemoveEnemy(e.gameObject);
                        Destroy(e.gameObject);
                    }
                    if (e.gameObject.tag == "Player") PlayerHealth.TakeDamage();
                }       
            }
            Destroy(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.L)) CheckForEnemies();
    }
    public void CheckForEnemies()
    {
        Collider2D[] allAttackables = Physics2D.OverlapBoxAll(transform.position, colliderSize, 0f, layer);
        if (allAttackables != null)
        {
            foreach (Collider2D e in allAttackables)
            {
                if (e.gameObject.tag == "Enemy")
                {
                    Debug.Log("Called this if check!");
                    status.TrapText();
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.localPosition, colliderSize);
    }
}
