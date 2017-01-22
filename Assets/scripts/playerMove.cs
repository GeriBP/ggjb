using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Assertions;


[RequireComponent(typeof(StudioEventEmitter))]
public class playerMove : MonoBehaviour {
    public float speed, smooth, pickUpPoints, riskPoints, speedPoints, maxScale, speedThreshhold, smoothWave;
    public cameraShake cShake;
    public gameManager GM;
    public Rigidbody2D myRb;
    public GameObject reconnectScreen, explosion, pickUpExp;
    public shockWave wave;
    private bool waveAvailable = true;
    private bool deathBool = false;
    public bool buttonBool = false;

    private StudioEventEmitter engineSoundEmitter;
    private StudioEventEmitter gunSoundEmitter;

	// Use this for initialization
	void Awake() 
    {
        myRb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(myRb);

        var soundEmitters = GetComponents<StudioEventEmitter>();
		Assert.IsTrue(soundEmitters.Length == 2, "Player requires two sound emitters");
        engineSoundEmitter = soundEmitters[0];
        gunSoundEmitter = soundEmitters[1];
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && !deathBool) //UP
        {
            myRb.AddForce(Vector2.up * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.S) && !deathBool) //DOWN
        {
            myRb.AddForce(Vector2.down * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.A) && !deathBool) //LEFT
        {
            myRb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.D) && !deathBool) //RIGHT
        {
            myRb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        }
        if (Input.GetKeyUp(KeyCode.R) && deathBool && buttonBool) //Reset
        {
            deathBool = false;
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            GM.reconnect();
            GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<TrailRenderer>().enabled = true;
            reconnectScreen.SetActive(false);
            buttonBool = false;
        }
        if (Input.GetKeyUp(KeyCode.Space) && waveAvailable && !deathBool) //ShockWave
        {
            StartCoroutine(waveCooldown());
            float meter = GM.waveBar.value;
            if(meter >= 100.0f)
            {
                //SPECIAL WAVE
                float proportion = meter / 100.0f;
                cShake.Shake(proportion);
                StartCoroutine(shootWave(proportion));
                GM.waveBar.value = 0;
            }
            else if(meter >= 1.0f) //10.0f is the minumum waveMeter you can spend
            {
                float proportion = meter / 100.0f;
                cShake.Shake(proportion);
                StartCoroutine(shootWave(proportion));
                GM.waveBar.value = 0;
            }
        }

        var overallVelocity = Mathf.Clamp01(myRb.velocity.magnitude / 8f);
        engineSoundEmitter.SetParameter("RPM", overallVelocity);
    }

    void FixedUpdate()
    {
        Vector2 v = myRb.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (myRb.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), smooth);
        }
        if (myRb.velocity.magnitude > speedThreshhold)
        {
            GM.addWave(speedPoints);
        }
    }

    public void TakeHit()
    {
        if(!deathBool) StartCoroutine(death());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PickUp")
        {
            GM.addWave(pickUpPoints);
            Instantiate(pickUpExp, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Risk" && myRb.velocity.magnitude > 3)
        {
            GM.addWave(riskPoints);
        }
    }

    IEnumerator shootWave(float proportion) //16.6f proportioon is max
    {
        gunSoundEmitter.Play();

        var w = Instantiate(wave, transform.position, Quaternion.identity);
        w.Damage = proportion;
        for (int i = 0; i < 50; ++i)
        {
            w.transform.localScale = Vector3.Lerp(w.transform.localScale, new Vector3(maxScale * proportion, maxScale * proportion, 0f), smoothWave * Time.deltaTime);
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
        GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        if(!deathBool) Instantiate(explosion, transform.position, Quaternion.identity);
        deathBool = true;
        StartCoroutine(screenDeath());
        yield return null;
    }
    IEnumerator screenDeath()
    {
        yield return new WaitForSeconds(2.0f);
        reconnectScreen.SetActive(true);
        buttonBool = true;
    }
    public void revive()
    {
        if (buttonBool) {
            deathBool = false;
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<TrailRenderer>().enabled = true;
            reconnectScreen.SetActive(false);
            buttonBool = false;
        }
    }
}
