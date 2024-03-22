using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstructions : MonoBehaviour
{
    public Player p;
    string[] words = { "null", "null"};
    string[] attackWords = { "null", "null", "null" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttackTheEnemy(string s)
    {
        if (Player.moved == true) return;
        else
        {
            p.AttackEnemy(s);
        }
    }
    public void DirectThePlayer(string s)
    {
        Array.Clear(words, 0, 2); //clears the array by getting the starting index and the number of elements to remove from that index
        words = s.Split(' ');
        Debug.Log(words[0]);
        Debug.Log(words[1]);
        if (words[1] == "medkit") p.Heal();
        else if (words[1] == "trap") p.TriggerTrip();
        else
        {
            if (int.Parse(words[1]) >= 4) return;
            StartCoroutine(p.moving(words[0], int.Parse(words[1])));
        }
    }
}
