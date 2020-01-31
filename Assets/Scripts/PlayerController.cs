using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float xSpeed, zSpeed;

    public GameObject bullet;
    public GameObject bulletP1;
    public Transform bulletSpawn;

    public float shootTime = 0.3f;
    float shootCounter = 0;

    public MeshRenderer sphere;
    public Light myLight;

    LaserTypes currentLaser = LaserTypes.Normal;

    public GameObject myCamera;

    bool player1 = false;
    Health myHealth;

    GameManager gm;

    private void Start() {
        if (isServer) {
            if (!isLocalPlayer) {
                player1 = false;
            }

            if (isLocalPlayer) {
                player1 = true;
                sphere.material.color = Color.blue;
                myLight.color = Color.blue;
            }
        }else {
            if (!isLocalPlayer) {
                player1 = true;
                sphere.material.color = Color.blue;
                myLight.color = Color.blue;
            }

            if (isLocalPlayer) {
                player1 = false;
            }
        }

        myHealth = GetComponent<Health>();
        myHealth.player1 = player1;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer) {
            return;
        }

        xSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
        zSpeed = Input.GetAxis("Vertical") * Time.deltaTime * 9;

        if (myHealth.speedBoost <= 0) {
            transform.Rotate(0, xSpeed, 0);
            transform.Translate(0, 0, zSpeed);
        }else {
            transform.Rotate(0, xSpeed * 1.5f, 0);
            transform.Translate(0, 0, zSpeed * 1.25f);
        }

        shootCounter += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && shootCounter > shootTime) {
            switch (currentLaser) {
                case LaserTypes.Normal:
                    CmdFire();
                    break;

                case LaserTypes.Spread:
                    CmdSpread();
                    break;
            }

            shootCounter = 0;
        }
    }

    [Command]
    void CmdFire() {
        GameObject spawnedBul = null;

        if (!player1) {
            spawnedBul = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        }
        else {
            spawnedBul = Instantiate(bulletP1, bulletSpawn.position, bulletSpawn.rotation);
        }

        NetworkServer.Spawn(spawnedBul);
    }

    [Command]
    void CmdSpread() {
        GameObject spawnedBulC = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        GameObject spawnedBulL = Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(bulletSpawn.eulerAngles.x, bulletSpawn.eulerAngles.y - 10, bulletSpawn.eulerAngles.z));
        GameObject spawnedBulR = Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(bulletSpawn.eulerAngles.x, bulletSpawn.eulerAngles.y + 10, bulletSpawn.eulerAngles.z));
        NetworkServer.Spawn(spawnedBulC);
        NetworkServer.Spawn(spawnedBulL);
        NetworkServer.Spawn(spawnedBulR);
    }

    public override void OnStartLocalPlayer() {
        myCamera.SetActive(true);

        if (isServer) {
            transform.position = GameObject.Find("Player1Pos").transform.position;
        }else {
            transform.position = GameObject.Find("Player2Pos").transform.position;
        }
    }
}
