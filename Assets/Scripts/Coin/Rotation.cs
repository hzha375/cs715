using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float degreesPerSec = 90f;

    void Update()
    {
        transform.Rotate(new Vector3(.0f, degreesPerSec * Time.deltaTime, .0f));
    }
}
