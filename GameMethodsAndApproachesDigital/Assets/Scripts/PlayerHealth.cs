using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    static int health = 3;
    public static int originalHealth = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void TakeDamage()
    {
        health--;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            //SceneManager.LoadScene("GameOver");
            Debug.Log("Dead");
        }
    }
    public static void setHealth(int h)
    {
        health = h;
        originalHealth = h;
    }
    public static int getHealth()
    {
        return health;
    }
}
