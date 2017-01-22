using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {
    public GameObject target;
    public playerMove pM;
    public float smooth, smoothIn, smoothOut;
    private float cameraZ;
    // Use this for initialization
    void Start () {
        cameraZ = transform.position.z;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!gameManager.bossDead) {
            if (pM.myRb.velocity.magnitude > 2.0f)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(0.0f, 0.0f, cameraZ), smooth);
                Camera.main.fov = Mathf.Lerp(Camera.main.fov, 88.0f, smoothOut);
            }
            else
            {
                Camera.main.fov = Mathf.Lerp(Camera.main.fov, 55.0f, smoothIn);
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, cameraZ), smooth);
                if (transform.position.x > 4.0f) transform.position = new Vector3(4.0f, transform.position.y, transform.position.z);
                else if (transform.position.x < -3.8f) transform.position = new Vector3(-3.8f, transform.position.y, transform.position.z);
                if (transform.position.y > 3.85f) transform.position = new Vector3(transform.position.x, 3.85f, transform.position.z);
                else if (transform.position.y < -3.67f) transform.position = new Vector3(transform.position.x, -3.67f, transform.position.z);
            }
        }
        else
        {
            transform.position =new Vector3(0.0f, 0.0f, cameraZ);
            Camera.main.fov = 88.0f;
        }
    }
}
