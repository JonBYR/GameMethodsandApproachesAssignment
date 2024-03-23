using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Debug.Log("Entered Wall");
            Enemy.threshold = Enemy.threshold / 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Debug.Log("Exited Wall");
            Enemy.threshold = Enemy.threshold * 2;
        }
    }
}
