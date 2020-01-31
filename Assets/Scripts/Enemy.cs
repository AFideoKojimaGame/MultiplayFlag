using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    GameObject[] players;
    float[] distances;

    public GameObject bullets;

    public float fireTimeMin = 0.5f;
    public float fireTimeMax = 1.5f;
    float fireTime;
    float fireCounter = 0;

    float lookTime = 6;
    float lookCounter = 0;

    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        fireTime = Random.Range(fireTimeMin, fireTimeMax);
        distances = new float[players.Length];
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < players.Length; i++) {
            distances[i] = Vector3.Distance(transform.position, players[i].transform.position);
            if(distances[i] < 1.5f) {
                //fire;
            }
        }

        //if(players.Length > 1) {
        //    lookCounter += Time.deltaTime;

        //}
	}
}
