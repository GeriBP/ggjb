using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormHole : MonoBehaviour {
    public GameObject enemy;
	// Use this for initialization
	void Start () {
        StartCoroutine("spawner");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator spawner()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject enemyInst = Instantiate(enemy, transform.position, Quaternion.identity);
       // enemyInst.transform.parent = transform;
        Destroy(gameObject);
    }
}
