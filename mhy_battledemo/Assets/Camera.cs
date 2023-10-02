using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Vector3 MousePos;
    public Transform CameraPos;
    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        Player.Rotate( -0.4f * Vector3.up * (MousePos - Input.mousePosition).x ) ;
        MousePos = Input.mousePosition;
    }
}
