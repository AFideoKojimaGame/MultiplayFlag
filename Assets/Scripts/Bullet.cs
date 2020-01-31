using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float moveSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * moveSpeed);
	}

    private void OnCollisionEnter(Collision col) {
        Destroy(gameObject);
    }
}
