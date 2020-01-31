using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Recovery : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 5, 0, Space.World);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            CmdHealed();
    }

    [Command]
    void CmdHealed() {
        Destroy(gameObject);
    }
}
