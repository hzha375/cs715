using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public int numberOfCoins;
    public GameObject coinPrefab;
    public GameObject coinParent;
    public GameObject player;
    public GameObject points;
    public float coinRotationDegreesPerSecond = 90f;

    public void Generate()
    {
        foreach (Transform child in coinParent.transform)
        {
            Destroy(child.gameObject);
        }


        for (int i = 0; i < numberOfCoins; i++)
        {
            GameObject newCoin = Instantiate(coinPrefab);
            newCoin.GetComponent<AcquireOnCollide>().player = player;
            newCoin.GetComponent<AcquireOnCollide>().points = points;
            // Place randomly now because CameraController needs these co-ordinates
            newCoin.GetComponent<RandomPlacement>().inMaze = GetComponent<MazeSpawner>();
            newCoin.GetComponent<RandomPlacement>().Place();
            newCoin.GetComponent<Rotation>().degreesPerSec = coinRotationDegreesPerSecond;

            // Reparent to the coin parent
            newCoin.transform.parent = coinParent.transform;
        }
    }
}
