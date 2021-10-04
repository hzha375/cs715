using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class Stopwatch : MonoBehaviour
{
    private bool running = false;
    private float value;
    public GameObject coinParent;
    public GameObject controller;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Run()
    {
        running = true;
    }

    public void ResetValue()
    {
        value = .0f;
    }

    public void Stop()
    {
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            value += Time.deltaTime;

            // If all coins are destroyed (collected)
            if (coinParent.transform.childCount == 0)
            {
                controller.GetComponent<CameraController>().gameOver();
            }
        }

        gameObject.GetComponent<Text>().text = System.Math.Round(value, 1).ToString();
    }
}
