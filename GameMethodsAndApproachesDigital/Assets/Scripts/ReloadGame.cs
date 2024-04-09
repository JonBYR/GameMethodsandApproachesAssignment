using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadGame : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("GameScene");
            canvas.SetActive(true);
            ReplaySystem.isPlaying = false;
        }
        if(ReplaySystem.isPlaying == true)
        {
            canvas.SetActive(false);
        }
    }
}
