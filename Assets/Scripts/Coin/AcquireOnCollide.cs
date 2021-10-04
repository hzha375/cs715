using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach this script to a GameObject to remove it and add a point when
 * a player collides with it.
 */
public class AcquireOnCollide : MonoBehaviour
{
    public GameObject player;
    public GameObject points;

    // Player has touched the coin, add one to their score
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            points.GetComponent<PlayerStats>().AddPoint();
            Destroy(gameObject);
        }
    }
}
