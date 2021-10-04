using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHorizontal : MonoBehaviour
{
    //Managing camera's horizontal movement;
    public float accelerate = 5f, rotateSpeed = 0.25f;
    public float moveAngle = 0f;
    public float horizontalInput;
    void Update(){
        horizontalInput = Input.GetAxis("Horizontal");
        moveAngle += horizontalInput * rotateSpeed;
        transform.Rotate(Vector3.up, moveAngle * accelerate * Time.deltaTime, Space.Self);
        moveAngle -= moveAngle * accelerate * Time.deltaTime;
    }
}
