using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {
    public GameObject target;
    public float smooth;
    private float cameraZ;
    // Use this for initialization
    void Start () {
        cameraZ = transform.position.z;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, cameraZ), smooth);
        if (transform.position.x > 4.0f) transform.position = new Vector3(4.0f, transform.position.y, transform.position.z);
        else if (transform.position.x < -3.8f) transform.position = new Vector3(-3.8f, transform.position.y, transform.position.z);
        if (transform.position.y > 3.85f) transform.position = new Vector3(transform.position.x, 3.85f, transform.position.z);
        else if (transform.position.y < -3.67f) transform.position = new Vector3(transform.position.x, -3.67f, transform.position.z);

    }
}
