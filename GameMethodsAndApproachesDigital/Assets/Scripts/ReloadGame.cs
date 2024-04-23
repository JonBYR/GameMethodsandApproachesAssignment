using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadGame : MonoBehaviour
{
    public GameObject canvas;
    public AudioSource replayMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("GameScene");
            EventSpace.trapTriggered = false;
            canvas.SetActive(true);
            ReplaySystem.isPlaying = false;
            PlayerHealth.setHealth(3);
            replayMusic.time = 0;
            replayMusic.Stop();
        }
        if(ReplaySystem.isPlaying == true)
        {
            canvas.SetActive(false);
        }
    }
}
