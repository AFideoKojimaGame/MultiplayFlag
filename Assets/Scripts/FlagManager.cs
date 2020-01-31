using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FlagManager : NetworkBehaviour {

    public GameObject flagPrefab;
    GameObject[] flagPos;

    public override void OnStartServer() {

        flagPos = GameObject.FindGameObjectsWithTag("FlagPos");

        int newIndex = Random.Range(0, flagPos.Length);
        Vector3 spawnPosition = flagPos[newIndex].transform.position;

        GameObject flag = Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(flag);
    }

    [Command]
    public void CmdRespawnFlag() {

        int newIndex = Random.Range(0, flagPos.Length);
        Vector3 spawnPosition = flagPos[newIndex].transform.position;

        GameObject flag = Instantiate(flagPrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(flag);
    }
}
