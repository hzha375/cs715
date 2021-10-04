using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera editCamera;
    public float zoomInput;
    public float accelerate = 5f, zoomSpeed = 0.1f;
    public float moveZoom = 0f;
    void Start()
    {
        editCamera = GetComponent<Camera>();
    }

    void Update()
    {
        zoomInput = Input.GetAxis("Zoom");
        moveZoom += zoomInput * zoomSpeed;
        editCamera.fieldOfView = editCamera.fieldOfView + moveZoom * accelerate * Time.deltaTime;
        if(editCamera.fieldOfView < 2)
        {
            editCamera.fieldOfView = 2;
            moveZoom = 0;
        }
        else if(editCamera.fieldOfView > 100)
        {
            editCamera.fieldOfView = 100;
            moveZoom = 0;
        }
        moveZoom -= moveZoom * accelerate * Time.deltaTime;
    }
}
