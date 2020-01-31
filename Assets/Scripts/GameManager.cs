using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Text text;

    bool someoneHasFlag = false;

    [SyncVar]
    string textToShow = "FIND THE FLAG\nBRING IT BACK TO YOUR BASE";

    public FlagManager fm;

    public GameObject buttons;

    [SyncVar]
    public int p1Victories = 0;
    [SyncVar]
    public int p2Victories = 0;

    public Text victories;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        text.text = textToShow;

        victories.text = "PLAYER 1: " + p1Victories + "\nPLAYER 2: " + p2Victories;
	}

    [Command]
    public void CmdFlagHolder(bool p1) {
        if(p1) {
            textToShow = "PLAYER 1 HAS THE FLAG";
        }else {
            textToShow = "PLAYER 2 HAS THE FLAG";
        }

        someoneHasFlag = true;
    }

    [Command]
    public void CmdWon(bool p1) {
        if (p1) {
            textToShow = "PLAYER 1 WINS\nPLAYER 2 LOSES";
            p1Victories++;
        }else {
            textToShow = "PLAYER 2 WINS\nPLAYER 1 LOSES";
            p2Victories++;
        }

        //if (p1)
        //    textToShow = "PLAYER 1 WINS\nPLAYER 2 LOSES";
        //else
        //    textToShow = "YOU WIN\nPLAYER 1 LOSES";

        someoneHasFlag = true;

        CmdRestartGame();
    }

    [Command]
    public void CmdLostFlag() {
        textToShow = "FIND THE FLAG\nBRING IT BACK TO YOUR BASE";
        someoneHasFlag = false;
    }

    [Command]
    public void CmdRestartGame() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        players[0].GetComponent<Health>().RpcRestartGame();
        players[1].GetComponent<Health>().RpcRestartGame();
        textToShow = "FIND THE FLAG\nBRING IT BACK TO YOUR BASE";
        fm.CmdRespawnFlag();
        someoneHasFlag = false;
    }
}
