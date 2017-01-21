using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {
    public int time;
    public Slider waveBar;
    public Text waveValue;
    public GameObject wave1;
    public GameObject[] waves;
    public int[] nEnemies;
    private int currentWave = 0;
    public static int enemiesAlive;
    private GameObject currentGo;
    // Use this for initialization
    void Start () {
        time = 0;
        //StartCoroutine("clockTick");
        enemiesAlive = nEnemies[currentWave];
        currentGo = Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        StartCoroutine("spawner");
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

    public void reconnect()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameObject inst = GameObject.Find("Shooter(Clone)");
        StopCoroutine("spawner");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Risk");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
  
        gameObjects = GameObject.FindGameObjectsWithTag("PickUp");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        Destroy(currentGo);
        if (currentWave > 0) --currentWave;
        enemiesAlive = nEnemies[currentWave];
        Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        StartCoroutine("spawner");
    }

    IEnumerator spawner()
    {
        if (enemiesAlive <= 0)
        {
            currentWave++;
            if (waves.Length > currentWave) {
                enemiesAlive = nEnemies[currentWave];
                Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            }
            else //end game
            {

            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("spawner");
    }
}
