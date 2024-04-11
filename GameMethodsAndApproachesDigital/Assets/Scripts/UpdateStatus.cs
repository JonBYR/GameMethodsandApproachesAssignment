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
    public void InvalidInput()
    {
        wallText.text = "To move please input a direction and the number of spaces\n";
        Invoke("WipeWall", 2f);
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
    public void CantAttack()
    {
        wallText.text = "Player will attack a wall\n";
        Invoke("WipeWall", 2f);
    }
    public void CantMove()
    {
        wallText.text = "Player can only move 3 spaces maximum\n";
        Invoke("WipeWall", 2f);
    }
    public void InvalidAttack()
    {
        wallText.text = "Player can only attack or pass\n";
        Invoke("WipeWall", 2f);
    }
    public void InvalidMis()
    {
        wallText.text = "Player can use a medkit or a trap\n";
        Invoke("WipeWall", 2f);
    }
    public void OverHealth()
    {
        wallText.text = "Already at max health\n";
        Invoke("WipeWall", 2f);
    }
    public void InCover(bool state)
    {
        if (state == true) information += "Currently in cover\n";
        else information += "Not in cover\n";
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
