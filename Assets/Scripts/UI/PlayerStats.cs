using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script attaches to a singular UI.Text object and keeps it updated
 * with the current score.
 */
public class PlayerStats : MonoBehaviour
{
    private int points = 0;
    public int targetPoints = 3;

    void Start()
    {
        UpdateDisplay();
    }

    /*
     * Make the text object reflect the current number of points
     */
    private void UpdateDisplay()
    {
        gameObject.GetComponent<Text>().text = points.ToString() + "/" + targetPoints.ToString();
    }

    public void Zero()
    {
        points = 0;
        UpdateDisplay();
    }

    /*
     * Add a point to the counter
     */
    public void AddPoint()
    {
        points += 1;
        UpdateDisplay();
    }
}
