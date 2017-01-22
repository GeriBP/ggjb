using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introSound : MonoBehaviour {
    private StudioEventEmitter emiter;
    // Use this for initialization
    void Start () {
        emiter.Play();
        emiter.SetParameter("Level", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
