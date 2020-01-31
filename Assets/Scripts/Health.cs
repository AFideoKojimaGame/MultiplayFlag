using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    [SyncVar]
    public int health = 3;
    public Slider healthSlider;

    [SyncVar]
    bool gotFlag = false;

    public bool player1 = false;

    Vector3 originalPos;

    public GameObject flag;

    GameManager gm;

    bool losing = false;

    public Slider speedSlider;

    [SyncVar]
    public float speedBoost = 0;

    public Text whatPlayer;

	// Use this for initialization
	void Start () {

        whatPlayer = GameObject.Find("WhatPlayer").GetComponent<Text>();

        if (isServer) {
            originalPos = GameObject.Find("Player1Pos").transform.position;
            whatPlayer.text = "YOU ARE PLAYER 1";
        }else {
            originalPos = GameObject.Find("Player2Pos").transform.position;
            whatPlayer.text = "YOU ARE PLAYER 2";
        }

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () { 

        healthSlider.value = health;

        if (health <= 0) {
            losing = true;

            if(gotFlag)
                CmdFlag();

            health = 3;
            RpcRespawn();
        }


        if (speedBoost > 0)
            speedBoost -= Time.deltaTime;

        speedSlider.value = speedBoost;

        if (speedSlider.value == 0)
            speedSlider.gameObject.SetActive(false);
        else
            speedSlider.gameObject.SetActive(true);
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Bullet")
            TakeDamage(-1);
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Recovery" && health < 3)
            TakeDamage(1);

        if (col.gameObject.tag == "Flag" && !gotFlag && health > 0) {
            gotFlag = true;
            gm.CmdFlagHolder(player1);
            NetworkServer.Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Player1Base" && player1 && gotFlag && !losing) {
            gm.CmdWon(player1);
            gotFlag = false;
        }

        if (col.gameObject.tag == "Player2Base" && !player1 && gotFlag && !losing) {
            gm.CmdWon(player1);
            gotFlag = false;
        }

        if (col.gameObject.tag == "SpeedBoost") {
            speedBoost = 10;
            NetworkServer.Destroy(col.gameObject);
        }
    }

    void TakeDamage(int dmg) {
        if (!isServer) {
            return;
        }

        health += dmg;
    }

    [Command]
    void CmdFlag() {
        GameObject newFlag = Instantiate(flag, transform.position, Quaternion.identity);
        NetworkServer.Spawn(newFlag);
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // move back to zero location

            transform.position = originalPos;

            if (gotFlag) {
                gotFlag = false;
                gm.CmdLostFlag();
            }

            losing = false;
        }
    }

    [ClientRpc]
    public void RpcRestartGame() {
        transform.position = originalPos;
        health = 3;
        gotFlag = false;
        losing = false;
        speedBoost = 0;
    }
}
