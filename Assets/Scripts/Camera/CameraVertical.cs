using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVertical : MonoBehaviour
{
    //Managing camera's vertical movement;
    public float accelerate = 5f, rotateSpeed = 0.25f;
    public float moveAngle = 0f;
    public float horizontalInput;
    private Vector2 currentRotation;
    void Update()
    {
        horizontalInput = Input.GetAxis("Vertical");
        moveAngle += horizontalInput * rotateSpeed;
        currentRotation.x += moveAngle * accelerate * Time.deltaTime;
        if(currentRotation.x < -44.9f)//Player can only have top view or flat view, Not using Mathf.Clamp to improve control experience
        {
            currentRotation.x = -44.9f;
            moveAngle = 0;
        }
        else if(currentRotation.x > 45)
        {
            currentRotation.x = 45;
            moveAngle = 0;
        }
        currentRotation.y = transform.rotation.y;
        transform.localRotation = Quaternion.Euler(currentRotation);
        moveAngle -= moveAngle * accelerate * Time.deltaTime;
    }
}
