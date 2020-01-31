using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserTypes {
    Normal,
    Spread
}

public class PlayerControllerOffline : MonoBehaviour {

    float xSpeed, zSpeed;

    public GameObject bullet;
    public Transform bulletSpawn;

    public float shootTime = 0.3f;
    float shootCounter = 0;

    LaserTypes currentLaser = LaserTypes.Normal;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        xSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * 150;
        zSpeed = Input.GetAxis("Vertical") * Time.deltaTime * 9;

        transform.Rotate(0, xSpeed, 0);
        transform.Translate(0, 0, zSpeed);

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

    void CmdFire() {
        GameObject spawnedBul = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
    }

    void CmdSpread() {
        GameObject spawnedBulC = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        GameObject spawnedBulL = Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(bulletSpawn.eulerAngles.x, bulletSpawn.eulerAngles.y - 10, bulletSpawn.eulerAngles.z));
        GameObject spawnedBulR = Instantiate(bullet, bulletSpawn.position, Quaternion.Euler(bulletSpawn.eulerAngles.x, bulletSpawn.eulerAngles.y + 10, bulletSpawn.eulerAngles.z));
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Spread") {
            currentLaser = LaserTypes.Spread;
        }

        if (col.gameObject.tag == "NormalLaser") {
            currentLaser = LaserTypes.Normal;
        }
    }
}
