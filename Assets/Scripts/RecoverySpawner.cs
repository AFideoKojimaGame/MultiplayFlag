using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class RecoverySpawner : NetworkBehaviour {

    public GameObject recovPrefab;
    public int numberOfRecovs = 8;
    GameObject[] recoveryPos;
    List<int> usedIndexes = new List<int>();
    bool repeat = false;

    public override void OnStartServer() {

        recoveryPos = GameObject.FindGameObjectsWithTag("RecovPos");

        for (int i = 0; i < numberOfRecovs; i++) {

            repeat = false;

            int newIndex = Random.Range(0, recoveryPos.Length);
            if (!usedIndexes.Contains(newIndex)) {

                usedIndexes.Add(newIndex);
                Vector3 spawnPosition = recoveryPos[newIndex].transform.position;

                GameObject recovery = Instantiate(recovPrefab, spawnPosition, Quaternion.identity);
                NetworkServer.Spawn(recovery);
            }
        }
    }
}