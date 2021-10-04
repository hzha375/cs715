using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Movement controller inspired by:
 * https://www.youtube.com/watch?v=_QajrabyTJc
 * 
 * Note that gravity is not yet implemented
 */

public class Movement : MonoBehaviour
{
    public CharacterController c;
    public float speed = 12;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        c.Move(move * speed * Time.deltaTime);
    }
}
