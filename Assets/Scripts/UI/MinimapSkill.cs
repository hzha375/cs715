using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSkill : MonoBehaviour
{
    // Start is called before the first frame update
    public float magicCastInput = 0;
    public Status playerStatus;
    void Start()
    {
        playerStatus = GameObject.Find("UI").GetComponent(typeof(Status)) as Status;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool currentStatus = (magicCastInput != 0);
        magicCastInput = Input.GetAxis("SummonMinimap");
        currentStatus = playerStatus.CastingMinimap(currentStatus);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(currentStatus);
        }
    }
}
