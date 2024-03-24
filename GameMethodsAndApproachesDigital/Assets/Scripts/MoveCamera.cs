using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position = new Vector3(495f, 3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
