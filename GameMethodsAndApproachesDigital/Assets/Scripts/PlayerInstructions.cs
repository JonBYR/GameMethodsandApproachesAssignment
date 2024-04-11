using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInstructions : MonoBehaviour
{
    public Player p;
    string[] words = { "null", "null"};
    string[] attackWords = { "null", "null", "null" };
    public UpdateStatus status;
    public GameObject mis;
    public GameObject attack;
    public GameObject move;
    public TileMap map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Miscell(TMP_InputField t)
    {
        string s = t.text;
        if (EventSystem.current.currentSelectedGameObject == mis)
        {
            Debug.Log(s);
            status.WipeString();
            ReplaySystem.recordTurn = true;
            if (s == "medkit") p.Heal();
            else if (s == "trap") { Debug.Log("Calling"); p.TriggerTrip(); }
            else
            {
                status.InvalidMis();
                return;
            }
        }
    }
    public void AttackTheEnemy(TMP_InputField t)
    {
        string s = t.text;
        if (EventSystem.current.currentSelectedGameObject == attack)
        {
            if (Player.moved == true) return;
            else
            {
                ReplaySystem.recordTurn = true;
                status.WipeString();
                p.AttackEnemy(s);
            }
        }
    }
    public void DirectThePlayer(TMP_InputField t)
    {
        if (EventSystem.current.currentSelectedGameObject == move)
        {
            string s = t.text;
            status.WipeString();
            Array.Clear(words, 0, 2); //clears the array by getting the starting index and the number of elements to remove from that index
            words = s.Split(' ');
            Debug.Log(words[0]);
            Debug.Log(words[1]);
            ReplaySystem.recordTurn = true;
            if (int.Parse(words[1]) >= 4)
            {
                status.CantMove();
                return; 
            }
            StartCoroutine(p.moving(words[0], int.Parse(words[1])));
        }
    }
}
