using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpace : MonoBehaviour
{
    public static bool triggerTrap = false;
    public static bool trapTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        triggerTrap = false;
        trapTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerTrap == false) return;
        else
        {
            if (collision.tag == "Player") PlayerHealth.TakeDamage();
            if (collision.tag == "Enemy") Destroy(collision.gameObject);
            Debug.Log("Called Trap");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerTrap == false) return;
        else
        {
            if (collision.tag == "Player") PlayerHealth.TakeDamage();
            if (collision.tag == "Enemy") Destroy(collision.gameObject);
            Debug.Log("Called Trap");
        }
    }
}
