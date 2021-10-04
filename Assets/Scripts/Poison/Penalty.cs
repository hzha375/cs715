using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penalty : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject statusObject;
    public GameObject hurtOverlay;

    public int penaltyPerSecond = 10;
    private bool touching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(playerObject))
        {
            touching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(playerObject)) {
           touching = false;
        }
    }

    private void Update()
    {
        if (touching)
        {
            Status status = statusObject.GetComponent<Status>();
            status.Hit();
        }
    }
}
