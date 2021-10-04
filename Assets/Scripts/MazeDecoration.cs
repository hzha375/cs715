using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDecoration : MonoBehaviour
{
    public GameObject torchParent;
    public GameObject torchPrefab;

    public GameObject pillarParent;
    public GameObject pillarPrefab;

    public GameObject wallDecorPrefab;
    public GameObject wallDecorParent;

    public GameObject tablePrefab;
    public GameObject tableParent;

    public GameObject chairPrefab;
    public GameObject chairParent;

    public GameObject mudWoodParent;

    public GameObject[] itemsToPickFrom;
    public GameObject[] itemsOnTable;
    
    public int torchHeight = 2;
    public int numToSpawnRandom = 5;

    public float raycastDistance;


    private void PlaceRandomObjects()
    {
        MazeSpawner spawner = GetComponent<MazeSpawner>();
        BasicMazeGenerator mazeGen = spawner.mMazeGenerator;

        // RaycastHit hit;

        for (int row = 0; row < mazeGen.RowCount; row++)
        {
            for (int col = 0; col < mazeGen.ColumnCount; col++)
            {
                // If not in path
                if (!mazeGen.GetMazeCell(row, col).InPath)
                {
                    // place tables and chairs
                    if (Random.Range(1, 5) == 1) 
                    { 
                        GameObject newTable = Instantiate(tablePrefab);
                        // GameObject newChair = Instantiate(chairPrefab);

                        // Set random rotation
                        newTable.transform.parent = tableParent.transform;
                        // newChair.transform.parent = chairParent.transform;

                        // Set position
                        Vector3 position = new Vector3(col * spawner.CellWidth, gameObject.transform.position.y, row * spawner.CellHeight);
                        newTable.transform.position = position;

                        // Set random Rotation
                        Quaternion randYRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        newTable.transform.rotation = randYRotation;

                        // If the table is picked, place items randomly on it
                        PlaceItemsOnTable(newTable);
                    
                    }

                    // place mud and stones
                    if (Random.Range(1, 3) == 1)
                    {
                        int randomPicker = Random.Range(0, itemsToPickFrom.Length);
                        GameObject newMudOrWood = Instantiate(itemsToPickFrom[randomPicker]);
                        newMudOrWood.transform.parent = mudWoodParent.transform;

                        // Set position
                        Vector3 position = new Vector3(col * spawner.CellWidth, gameObject.transform.position.y, row * spawner.CellHeight);
                        newMudOrWood.transform.position = position;

                        // Set random Rotation
                        Quaternion randYRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        newMudOrWood.transform.rotation = randYRotation;
                    }
                }
            }
        }
    }

    // %%%%%% need to be fixed 
    private void PlaceItemsOnTable(GameObject table)
    {
        RaycastHit hit;
        Vector3 center = table.GetComponent<Renderer>().bounds.center;

        if (Physics.Raycast(center + new Vector3(0, 5, 0), Vector3.down, out hit, 5))
        {
            int randomNum = 3;
            for (int i = 0; i < randomNum; i++)
            {
                int randomPicker = Random.Range(0, itemsOnTable.Length);
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                GameObject randomItemOnTable = Instantiate(itemsOnTable[randomPicker], hit.point, spawnRotation);
                randomItemOnTable.transform.parent = table.transform;
            }
        }
        // Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        // Instantiate(object, hit.point, spawnRotation);
        // get the size of the table
        //float width = table.GetComponent<Renderer>().bounds.size.x;
        //float length = table.GetComponent<Renderer>().bounds.size.z;
        //float height = table.GetComponent<Renderer>().bounds.size.y;

        // Set the positions of items to put on table
    }

    private GameObject PlaceTorch(float x, float z, float rotation)
    {
        GameObject newTorch = Instantiate(torchPrefab);
        newTorch.transform.position = new Vector3(x, torchHeight, z);
        newTorch.transform.parent = torchParent.transform;
        newTorch.transform.Rotate(Vector3.up, rotation);
        return newTorch;
    }

    private GameObject PlaceWallDecor(float x, float z, float rotation)
    {
        /*
        GameObject newWallDecor = Instantiate(wallDecorPrefab);
        newWallDecor.transform.position = new Vector3(x, torchHeight, z);
        newWallDecor.transform.parent = wallDecorParent.transform;
        newWallDecor.transform.Rotate(Vector3.up, rotation);
        return newWallDecor;*/
        return null;
    }

    private void PlaceTorches()
    {
        MazeSpawner mazeSpawner = gameObject.GetComponent<MazeSpawner>();
        BasicMazeGenerator mazeGen = mazeSpawner.mMazeGenerator;

        // Check for two horizontal walls in a row
        for (int row = 0; row < mazeGen.RowCount; row++)
        {
            for (int col = 1; col < mazeGen.ColumnCount; col += 2)
            {
                // If this cell has a back wall and the lat column had one too
                if (mazeGen.GetMazeCell(row, col).WallBack && mazeGen.GetMazeCell(row, col - 1).WallBack)
                {

                    // Place on wall
                    float x = col * mazeSpawner.CellWidth;
                    float z = row * mazeSpawner.CellHeight - mazeSpawner.CellHeight / 2;
                    PlaceTorch(x, z, 0f);
                    PlaceWallDecor(x, z, 0f);
                }

                if (mazeGen.GetMazeCell(row, col).WallFront && mazeGen.GetMazeCell(row, col - 1).WallFront)
                {
                    float x = col * mazeSpawner.CellWidth;
                    float z = row * mazeSpawner.CellHeight + mazeSpawner.CellHeight / 2;
                    PlaceTorch(x, z, 180f);
                    PlaceWallDecor(x, z, 180f);
                }
            }
        }

        // Check for two vertical walls in a row
        for (int row = 1; row < mazeGen.RowCount; row += 2)
        {
            for (int col = 0; col < mazeGen.ColumnCount; col++)
            {
                // If this cell has a back wall and the lat column had one too
                if (mazeGen.GetMazeCell(row, col).WallLeft && mazeGen.GetMazeCell(row - 1, col).WallLeft)
                {

                    // Place on wall
                    float x = col * mazeSpawner.CellWidth - mazeSpawner.CellWidth / 2;
                    float z = row * mazeSpawner.CellHeight;
                    PlaceTorch(x, z, 90f);
                    PlaceWallDecor(x, z, 90f);
                }

                if (mazeGen.GetMazeCell(row, col).WallRight && mazeGen.GetMazeCell(row - 1, col).WallRight)
                {
                    float x = col * mazeSpawner.CellWidth + mazeSpawner.CellWidth / 2;
                    float z = row * mazeSpawner.CellHeight;
                    PlaceTorch(x, z, -90f);
                    PlaceWallDecor(x, z, -90f);
                }
            }
        }
    }


    private GameObject PlacePillar(float x, float z)
    {
        GameObject newPillar = Instantiate(pillarPrefab);
        newPillar.transform.position = new Vector3(x, 0, z);
        newPillar.transform.parent = pillarParent.transform;
        return newPillar;
    }

    private void PlacePillars()
    {
        MazeSpawner mazeSpawner = gameObject.GetComponent<MazeSpawner>();
        BasicMazeGenerator mazeGen = mazeSpawner.mMazeGenerator;

        // Check for corners
        for (int row = 0; row < mazeGen.RowCount; row++)
        {
            for (int col = 0; col < mazeGen.ColumnCount; col++)
            {
                // Left back
                if (mazeGen.GetMazeCell(row, col).WallLeft && mazeGen.GetMazeCell(row, col).WallBack)
                {
                    float x = col * mazeSpawner.CellWidth - mazeSpawner.CellWidth/2;
                    float z = row * mazeSpawner.CellHeight - mazeSpawner.CellHeight/2;
                    PlacePillar(x, z);
                }

                // Right front
                if (mazeGen.GetMazeCell(row, col).WallRight && mazeGen.GetMazeCell(row, col).WallFront)
                {
                    float x = col * mazeSpawner.CellWidth + mazeSpawner.CellWidth / 2;
                    float z = row * mazeSpawner.CellHeight + mazeSpawner.CellHeight / 2;
                    PlacePillar(x, z);
                }

                // Left front
                if (mazeGen.GetMazeCell(row, col).WallLeft && mazeGen.GetMazeCell(row, col).WallFront)
                {
                    float x = col * mazeSpawner.CellWidth - mazeSpawner.CellWidth / 2;
                    float z = row * mazeSpawner.CellHeight + mazeSpawner.CellHeight /2;
                }

                // Right back
                if (mazeGen.GetMazeCell(row, col).WallRight && mazeGen.GetMazeCell(row, col).WallBack)
                {
                    float x = col * mazeSpawner.CellWidth + mazeSpawner.CellWidth / 2;
                    float z = row * mazeSpawner.CellHeight - mazeSpawner.CellHeight / 2;
                    PlacePillar(x, z);
                }
            }
        }
    }

    // Start is called before the first frame update
    public void Decorate()
    {
        foreach (Transform child in torchParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in pillarParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in tableParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in mudWoodParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in wallDecorParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        PlaceTorches();
        PlacePillars();
        PlaceRandomObjects();
    }
}
