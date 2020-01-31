using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SpeedManager : NetworkBehaviour
{

    public GameObject speedPrefab;
    public int numberOfSpeed = 3;
    GameObject[] speedPos;
    List<int> usedIndexes = new List<int>();
    bool repeat = false;

    public override void OnStartServer()
    {

        speedPos = GameObject.FindGameObjectsWithTag("SpeedPos");

        for (int i = 0; i < numberOfSpeed; i++)
        {

            repeat = false;

            int newIndex = Random.Range(0, speedPos.Length);
            if (!usedIndexes.Contains(newIndex))
            {

                usedIndexes.Add(newIndex);
                Vector3 spawnPosition = speedPos[newIndex].transform.position;

                GameObject sp = Instantiate(speedPrefab, spawnPosition, Quaternion.identity);
                NetworkServer.Spawn(sp);
            }
        }
    }
}