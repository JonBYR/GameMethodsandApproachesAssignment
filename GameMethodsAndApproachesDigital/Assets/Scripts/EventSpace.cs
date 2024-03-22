using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpace : MonoBehaviour
{
    public static bool triggerTrap = false;
    public static bool trapTriggered = false;
    public LayerMask layer;
    public UpdateStatus status;
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
            Collider2D[] allAttackables = Physics2D.OverlapBoxAll(transform.localPosition, new Vector2(6, 6), 0f, layer);
            if(allAttackables != null)
            {
                foreach(Collider2D e in allAttackables)
                {
                    if (e.gameObject.tag == "Enemy") Destroy(e.gameObject);
                    if (e.gameObject.tag == "Player") PlayerHealth.TakeDamage();
                }       
            }
            Destroy(this.gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.localPosition, new Vector2(6, 6));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            status.TrapText();
        }
    }
}
