using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public float maxHP = 100;
    public float startHP = 100;
    public float currentHP;
    public float maxMP = 10;
    public float startMP = 10;
    public float currentMP;
    public float hitAmount;

    // HP reduction per second
    public float HPRestoreSpeed;

    // MP increase and decrease rates
    public float MPRestoreSpeed;
    public float MPDrainSpeed;
    public bool drainMP = false;
    public bool drainHealth = false;

    // Magic cast requirement
    public float MinimapRequire = 1;
    public float MinimapCost = 1;

    public Slider HPSlider;
    public Slider MPSlider;
    public GameObject controller;
    public GameObject hitOverlay;

    // Player damage from poisons
    private float hitStatus;
    public float hitDrainRate = 1;

    // Start is called before the first frame update
    public void Start()
    {
        currentHP = startHP;
        currentMP = startMP;
        hitStatus = 0f;
    }

    void Update()
    {
        currentMP += Time.deltaTime * (drainMP ? -MPDrainSpeed : MPRestoreSpeed);
        currentHP += Time.deltaTime * (drainHealth ? HPRestoreSpeed : 0);

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if(currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        
        if(currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        
        if(currentMP <= 0)
        {
            currentMP = 0;
            DrainMP();
        }
        
        HPSlider.value = GetHPRatio();
        MPSlider.value = GetMPRatio();

        hitStatus -= hitDrainRate * Time.deltaTime;
        hitStatus = (hitStatus < 0f) ? 0f : hitStatus;

        // Set alpha for hit overlay
        Color col = hitOverlay.GetComponent<Image>().color;
        col.a = hitStatus;
        hitOverlay.GetComponent<Image>().color = col;
    }
    // to change the value

    public float GetHPRatio()
    {
        if(maxHP == 0)
        {
            return 0;
        }
        return currentHP / maxHP;
    }

    public float GetMPRatio()
    {
        if (maxMP == 0)
        {
            return 0;
        }
        return currentMP / maxMP;
    }

    public void Die()
    {
        controller.GetComponent<CameraController>().gameOver();
    }

    // Switch to first person once MP is drained
    public void DrainMP()
    {
        drainMP = false;
    }

    // Register a hit from an object
    public void Hit()
    {
        // If vulnerable
        if (hitStatus <= 0f)
        {
            // Remove health and reset status
            currentHP -= hitAmount;
            hitStatus = 1f;
        }
    }

    public bool CastingMinimap(bool status)
    {
        if (drainMP)
        {
            drainMP = status;
            return status;
        }
        if(!drainMP & status & currentMP > MinimapRequire)
        {
            currentMP -= MinimapCost;
            drainMP = true;
            return true;
        }
        drainMP = false;
        return false;
    }
}
