using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockWave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        IEnemy ie = other.gameObject.GetComponent<IEnemy>();
        if (ie != null)
        {
            ie.TakeHit();
        }
    }
}
