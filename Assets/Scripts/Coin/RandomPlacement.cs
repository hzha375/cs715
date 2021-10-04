using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    // Thing to place on
    public MazeSpawner inMaze;
    public int col;
    public int row;

    // Pick a random place on the baseplate for this GameObject when the game starts
    public void Place()
    {
        // Randomly generate a cell position until we find one that's unoccupied
        col = Mathf.RoundToInt(Random.Range(0, inMaze.Columns));
        row = Mathf.RoundToInt(Random.Range(0, inMaze.Rows));
        
        Vector3 randomVec = new Vector3(col*inMaze.CellWidth, gameObject.transform.position.y, row*inMaze.CellHeight);
        gameObject.transform.position = randomVec;
    }
}
