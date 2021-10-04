using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGenerator : MonoBehaviour
{
    public GameObject poisonPrefab;
    public GameObject poisonParent;
    public GameObject player;
    public GameObject statusObject;
    public GameObject hurtOverlay;

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void Generate()
    {
        MazeSpawner spawner = GetComponent<MazeSpawner>();
        BasicMazeGenerator mazeGen = spawner.mMazeGenerator;

        // Remove the old poison
        foreach (Transform t in poisonParent.transform)
        {
            Destroy(t.gameObject);
        }

        // Highlight all unnecessary locations
        for (int row = 0; row < mazeGen.RowCount; row++)
        {
            for (int col = 0; col < mazeGen.ColumnCount; col++)
            {
                // If not in path
                if (!mazeGen.GetMazeCell(row, col).InPath)
                {
                    // 1 in 4 chance of being poison
                    if (Random.Range(1, 4) == 1) { 
                        // Create poison
                        GameObject poison = Instantiate(poisonPrefab);
                        Penalty penalty = poison.GetComponent<Penalty>();
                        // Set player variable
                        penalty.playerObject = player;
                        // Set status object
                        penalty.statusObject = statusObject;
                        // Reparent to poisonParent
                        poison.transform.parent = poisonParent.transform;

                        // Set layer to the layer of the parent
                        SetLayerRecursively(poison, poisonParent.layer);

                        // Set position
                        Vector3 position = new Vector3(col * spawner.CellWidth, gameObject.transform.position.y, row * spawner.CellHeight);
                        poison.transform.position = position;
                    }
                }
            }
        }
    }
}
