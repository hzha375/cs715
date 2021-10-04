using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script that determines which camera is active
 */
public class CameraController : MonoBehaviour
{
    public Camera editorCam;
    public GameObject stopwatch;
    public GameObject player;
    public GameObject mazeLevel;
    public GameObject points;
    public GameObject statusObject;
    public GameObject coinParent;
    public GameObject poisionPrefab;
    private bool playing = false;
    private Stopwatch swInstance;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
        updateCams();
        swInstance = stopwatch.GetComponent<Stopwatch>();
    }

    private void updateCams()
    {
        editorCam.gameObject.SetActive(!playing);
        player.SetActive(playing);
        Cursor.lockState = playing ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void GenerateMaze()
    {
        // Generate new level
        mazeLevel.GetComponent<MazeSpawner>().Spawn();
        // Decorate maze
        mazeLevel.GetComponent<MazeDecoration>().Decorate();
        // Generate new coins, placing immediately
        mazeLevel.GetComponent<CoinGenerator>().Generate();

        // Coins + player
        Vector2[] points = new Vector2[coinParent.transform.childCount + 1];

        int index = 0;
        foreach (Transform o in coinParent.transform)
        {
            RandomPlacement placement = o.gameObject.GetComponent<RandomPlacement>();
            points[index] = new Vector2(placement.row, placement.col);
            index++;
        }

        points[index] = PlacePlayer();
        // Mark all necessary locations in the maze
        MarkPath.Mark(mazeLevel.GetComponent<MazeSpawner>().mMazeGenerator, points);

        // Generate poison based on this
        mazeLevel.GetComponent<PoisonGenerator>().Generate();
    }

    private Vector2 PlacePlayer()
    {
        // Randomly place player
        MazeSpawner maze = mazeLevel.GetComponent<MazeSpawner>();
        int col = Random.Range(0, maze.Columns - 1);
        int row = Random.Range(0, maze.Rows - 1);

        CharacterController c = player.GetComponent<Movement>().c;

        c.enabled = false;
        c.transform.position = new Vector3(col * maze.CellWidth, player.transform.position.y, row * maze.CellHeight);
        c.enabled = true;

        return new Vector2(row, col);
    }

    private void startGame()
    {
        points.GetComponent<PlayerStats>().Zero();

        swInstance.ResetValue();
        swInstance.Run();
    }

    public void gameOver()
    {
        GenerateMaze();
        playing = false;
        swInstance.Stop();

        statusObject.GetComponent<Status>().Start();
        updateCams();
    }

    // The tab key switches perspective
    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            if (!playing)
            {
                startGame();
            }

            playing = true;
            updateCams();
        }

        // Drain MP if we're playing the game and in thrid person
        statusObject.GetComponent<Status>().drainHealth = playing;
    }
}
