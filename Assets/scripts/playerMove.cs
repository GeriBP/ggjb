using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Assertions;

public class playerMove : MonoBehaviour {
    public float speed, smooth, pickUpPoints, maxScale;
    public cameraShake cShake;
    public gameManager GM;
    private Rigidbody2D myRb;
    public GameObject reconnectScreen, explosion;
    public shockWave wave;
    private bool waveAvailable = true;

    private StudioEventEmitter soundEmitter;

	// Use this for initialization
	void Awake() 
    {
        myRb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(myRb);

        soundEmitter = GetComponent<StudioEventEmitter>();
        Assert.IsNotNull(soundEmitter);
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
                cShake.Shake(proportion);
                StartCoroutine("shootWave", proportion);
                GM.waveBar.value = 0;
            }
            else if(meter >= 1.0f) //10.0f is the minumum waveMeter you can spend
            {
                float proportion = meter / 100.0f;
                cShake.Shake(proportion);
                StartCoroutine("shootWave", proportion);
                GM.waveBar.value = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            soundEmitter.SetParameter("Prueba_1", 2f);
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
        StartCoroutine("death");
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
        var w = Instantiate(wave, transform.position, Quaternion.identity);
        w.Damage = proportion;
        for (int i = 0; i < 50; ++i)
        {
            w.transform.localScale = Vector3.Lerp(w.transform.localScale, new Vector3(maxScale * proportion, maxScale * proportion, 0f), smooth * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(w.gameObject);
    }

    IEnumerator waveCooldown()
    {
        waveAvailable = false;
        yield return new WaitForSeconds(2.0f);
        waveAvailable = true;
    }
    IEnumerator death()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        Instantiate(explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.0f);
        reconnectScreen.SetActive(true);
    }
}
