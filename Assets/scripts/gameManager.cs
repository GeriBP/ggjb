using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class gameManager : MonoBehaviour {

    [SerializeField]
    private GameObject normalBackground, cutsceneBackground;

    public int time;
    public Canvas canvas;
    public Slider waveBar;
    public Text waveValue;
    public GameObject wave1;
    public GameObject[] waves;
    public int[] nEnemies;
    private int currentWave = 0;
    public static int enemiesAlive;
    private GameObject currentGo;
    public playerMove player;
    public static bool bossDead = false;
    private StudioEventEmitter emiter;
    // Use this for initialization
    void Start () {
        time = 0;
        //StartCoroutine("clockTick");
        emiter = GetComponent<StudioEventEmitter>();
        emiter.SetParameter("Level",currentWave+1);
        enemiesAlive = nEnemies[currentWave];
        currentGo = Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        StartCoroutine("spawner");
    }
	
	// Update is called once per frame
	void Update () {
        waveValue.text = waveBar.value.ToString();

        // Debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (var enemy in FindObjectsOfType<Drone>())
            {
                enemy.TakeHit(9999f);
            }
            foreach (var enemy in FindObjectsOfType<Shooter>())
            {
                enemy.TakeHit(9999f);
            }
            foreach (var enemy in FindObjectsOfType<Kamikaze>())
            {
                enemy.TakeHit(9999f);
            }
        }
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
        if (currentWave > 0)
        {
            --currentWave;
            emiter.SetParameter("Level", currentWave + 1);
        }
        enemiesAlive = nEnemies[currentWave];
        Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        StartCoroutine("spawner");
        player.revive();
    }

    IEnumerator spawner()
    {
        if (enemiesAlive <= 0)
        {
            currentWave++;
            if (waves.Length > currentWave) {
                emiter.SetParameter("Level", currentWave + 1);
                enemiesAlive = nEnemies[currentWave];
                Instantiate(waves[currentWave], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            }
            else //end game
            {
                canvas.enabled = false;
                bossDead = true;
                Debug.Log("End game");
        
               GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PickUp");

                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObjects[i]);
                }
                Destroy(GameObject.Find("finalShockW(Clone)"));

                normalBackground.SetActive(false);
                cutsceneBackground.SetActive(true);
                player.gameObject.SetActive(false);
                StartCoroutine(GoToIntroAfterDelay());

                yield return null;
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("spawner");

    }

    IEnumerator GoToIntroAfterDelay()
    {
        yield return new WaitForSeconds(11f);
        SceneManager.LoadScene(0);
    }
}
