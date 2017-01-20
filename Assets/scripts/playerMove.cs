using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
    public float speed, smooth, pickUpPoints;
    public gameManager GM;
    private Rigidbody2D myRb;
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
        if (Input.GetKey(KeyCode.Space)) //ShockWave
        {
            
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
        }
    }
}
