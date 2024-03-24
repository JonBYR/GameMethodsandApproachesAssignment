using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReplaySystem : MonoBehaviour
{
    Camera thisCam;
    Camera mainCamera;
    public float delayBetweenFrames = 0.05f;
    private List<Texture2D> frames = new List<Texture2D>();
    private bool isRecording = false;
    private bool isPlaying = false;
    public Renderer squareRender;
    public static bool recordTurn = false;
    //https://www.youtube.com/watch?v=iNVnWLKUKw4
    // Start is called before the first frame update
    void Start()
    {
        thisCam = GameObject.Find("Camera").GetComponent<Camera>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void StartRecording()
    {
        Debug.Log("Recording");
        frames.Clear();
        isRecording = true;
    }
    public void StopRecording()
    {
        Debug.Log("Stop Recording");
        isRecording = false;
    }
    void RecordFrame()
    {
        if(isRecording)
        {
            Texture2D frame = CaptureFrame();
            frames.Add(frame);
        }
    }
    Texture2D CaptureFrame()
    {
        RenderTexture currentTexture = RenderTexture.active; //saves current texture to be restored later
        RenderTexture.active = thisCam.targetTexture; //active texture is applied to the camera texture (for rendering)
        Texture2D frame = new Texture2D(thisCam.targetTexture.width, thisCam.targetTexture.height); //dimensions for image
        frame.ReadPixels(new Rect(0, 0, thisCam.targetTexture.width, thisCam.targetTexture.height), 0, 0); //reads pixels from camera texture to apply to the frame
        frame.Apply();
        RenderTexture.active = currentTexture; //reset texture
        return frame;
    }
    public void StartPlayback()
    {
        Debug.Log("Start Looking");
        if (!isPlaying && frames.Count > 0)
        {
            mainCamera.transform.position = new Vector3(163f, 83f, 0f);
            StartCoroutine(Playback());
        }
            
    }
    IEnumerator Playback()
    {
        isPlaying = true;
        for(int i = 0; i < frames.Count; i++)
        {
            DisplayFrame(frames[i]);
            yield return new WaitForSeconds(delayBetweenFrames);
        }
        isPlaying = false;
    }
    void DisplayFrame(Texture2D frame)
    {
        squareRender.material.mainTexture = frame;
    }
    // Update is called once per frame
    void FixedUpdate() //fixed Update is set at a specific value
    {
        if(recordTurn == true) RecordFrame(); //if the player/enemy is moving record the frame so that the list is not filled with static textures
    }
}
