using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject fpc;
    public static Main _instance;
    public Transform[] points;
    public int point_id = 1;
    public Text level;
    void Start()
    {
        _instance = this;
    }

    public void portal(int floorNumber)
    {
        SetPos(points[point_id].position);
        level.text = "F" + floorNumber.ToString();
        point_id++;
    }
    void SetPos(Vector3 pos)
    {
        fpc.GetComponent<CharacterController>().enabled = false;
        fpc.GetComponent<FirstPersonController>().enabled = false;
     
        Invoke("setPos", 0.3f);
    }

    public void setPos()
    {
        fpc.transform.position = points[point_id].position;
        fpc.GetComponent<FirstPersonController>().enabled = true;
        fpc.GetComponent<CharacterController>().enabled = true;
        point_id++;
    } 
}
