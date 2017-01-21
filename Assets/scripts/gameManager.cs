﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
    public int time;
    public Slider waveBar;
    public Text waveValue;
    public GameObject wave1;
	// Use this for initialization
	void Start () {
        time = 0;
        StartCoroutine("clockTick");
    }
	
	// Update is called once per frame
	void Update () {
        waveValue.text = waveBar.value.ToString();
    }

    IEnumerator clockTick()
    {
        ++time;
        //waveBar.value++; //Descrease/increase wave with time
        if (time == 4.0f)//example wave
        {
            Instantiate(wave1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        }
        if (time == 50.0f)//example wave
        {
            Instantiate(wave1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("clockTick");
    }

    public void addWave(float n)
    {
        float actual = waveBar.value;
        actual += n;
        if (actual > 100) actual = 100;
        waveBar.value = actual;
    }
}
