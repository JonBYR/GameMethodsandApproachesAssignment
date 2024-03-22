using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitController : MonoBehaviour
{
    public Player p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Player Detected");
            if (p.medkits > 0) return;
            else
            {
                Debug.Log("Should call this");
                p.medkits++;
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player Detected");
            if (p.medkits > 0) return;
            else
            {
                Debug.Log("Should call this");
                p.medkits++;
                Destroy(this.gameObject);
            }
        }
    }
}
