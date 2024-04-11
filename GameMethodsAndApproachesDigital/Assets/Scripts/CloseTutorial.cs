using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorial : MonoBehaviour
{
    public GameObject tutorial;
    public void closeTutorial()
    {
        tutorial.SetActive(false);
    }
}
