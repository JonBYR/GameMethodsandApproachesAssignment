using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateStatus : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI wallText;
    public string information;
    // Start is called before the first frame update
    void Start()
    {
        WipeString();
    }
    public void WipeString()
    {
        information = "";
    }
    public void EnemyFound(string direction)
    {
        information += "Enemy found " + direction + " of player\n";
    }
    public void AttackableEnemy()
    {
        information += "An enemy can be attacked!\n";
    }
    public void HealthOfPlayer()
    {
        information += "Current player health is: " + PlayerHealth.getHealth() + "\n";
    }
    public void CurrentHealthKits(int healthKits)
    {
        information += "Current health kits: " + healthKits + "\n";
    }
    public void WhereIsHealthKit(string direction)
    {
        information += "There is a healthkit " + direction + " you\n";
    }
    public void TrapText()
    {
        information += "There is an enemy to defeat with a trap!\n";
    }
    public void HitWall()
    {
        wallText.text = "Player will hit a wall\n";
        Invoke("WipeWall", 2f);
    }
    public void DisplayAllInfo()
    {
        stateText.text = information;
    }
    void WipeWall()
    {
        wallText.text = "";
    }
}
