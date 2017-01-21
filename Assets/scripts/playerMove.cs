﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
    public float speed, smooth, pickUpPoints, maxScale;
    public gameManager GM;
    private Rigidbody2D myRb;
    public GameObject wave;
    private bool waveAvailable = true;
	// Use this for initialization
	void Start () {
        myRb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W)) //UP
        {
            myRb.AddForce(Vector2.up * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.S)) //DOWN
        {
            myRb.AddForce(Vector2.down * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.A)) //LEFT
        {
            myRb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.D)) //RIGHT
        {
            myRb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        }
        if (Input.GetKeyUp(KeyCode.Space) && waveAvailable) //ShockWave
        {
            StartCoroutine("waveCooldown");
            float meter = GM.waveBar.value;
            if(meter >= 100.0f)
            {
                //SPECIAL WAVE
                float proportion = meter / 100.0f;
                StartCoroutine("shootWave", proportion);
                GM.waveBar.value = 0;
            }
            else if(meter >= 10.0f) //10.0f is the minumum waveMeter you can spend
            {
                float proportion = meter / 100.0f;
                StartCoroutine("shootWave", proportion);
                GM.waveBar.value = 0;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 v = myRb.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (myRb.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), smooth);
        }
        if (myRb.velocity.magnitude > 3)
        {
            GM.addWave(0.01f);
        }
    }

    public void TakeHit()
    {   
        // Die after 1 hit.
        Debug.Log("Game over");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PickUp")
        {
            GM.addWave(pickUpPoints);
            //Instantiate enemy death anim
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Risk" && myRb.velocity.magnitude > 3)
        {
            GM.addWave(5.0f);
        }
    }

    IEnumerator shootWave(float proportion) //16.6f proportioon is max
    {
        GameObject w = Instantiate(wave, transform.position, Quaternion.identity) as GameObject;
        for (int i = 0; i < 200; ++i)
        {
            w.transform.localScale = Vector3.Lerp(w.transform.localScale, new Vector3(maxScale * proportion, maxScale * proportion, 0f), smooth * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(w);
    }

    IEnumerator waveCooldown()
    {
        waveAvailable = false;
        yield return new WaitForSeconds(2.0f);
        waveAvailable = true;
    }
}
