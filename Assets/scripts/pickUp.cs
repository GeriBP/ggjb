using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour {
    private Rigidbody2D myRb;
    public float minSpeed, maxSpeed;
    // Use this for initialization
    void Start()
    {
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        myRb = GetComponent<Rigidbody2D>();
        myRb.AddForce(Random.insideUnitCircle * Random.Range(minSpeed, maxSpeed), ForceMode2D.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
