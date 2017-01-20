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
	}
}
