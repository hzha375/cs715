using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hurt : MonoBehaviour
{
    private float A;
    public float alphaRate = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        A = 0f;
    }

    public void Hit()
    {
        if (A <= 0f)
        {
            A = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        A -= alphaRate * Time.deltaTime;
        A = (A < 0f) ? 0f : A;

        // Make alpha adjustments
        Color col = gameObject.GetComponent<Image>().color;
        col.a = A;
        gameObject.GetComponent<Image>().color = col;
    }
}
