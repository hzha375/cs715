using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public int floorNumber = 1;
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag=="Player")
        {
            if(this.gameObject.name== Main._instance.point_id.ToString())
            {

                Main._instance.portal(floorNumber);
            }
        }
    }

}
