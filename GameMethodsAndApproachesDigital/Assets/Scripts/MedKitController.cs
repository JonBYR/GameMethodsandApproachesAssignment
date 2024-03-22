using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitController : MonoBehaviour
{
    public Player p;
    public UpdateStatus status;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MedKit()
    {
        Vector3 difference = transform.position - p.transform.position;
        float xDiff = difference.x;
        float yDiff = difference.y;
        if (Mathf.Abs(xDiff) >= Mathf.Abs(yDiff))
        {
            if (xDiff < 0) status.WhereIsHealthKit(" Left of");
            else status.WhereIsHealthKit(" Right of");
        }
        else if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
        {
            if (yDiff < 0) status.WhereIsHealthKit(" Below");
            else status.WhereIsHealthKit(" Above");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
            if (p.medkits > 0) return;
            else
            {
                
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
