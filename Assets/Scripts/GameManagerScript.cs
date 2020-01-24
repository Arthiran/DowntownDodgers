using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private GameObject PlayerInstance;
    [Range(1,4)]
    public int NumberOfPlayers = 1;
    public GameObject PlayerPrefab;
    private GameObject[] LootSpawnList;

    public GameObject DodgeballLootPrefab;

    private int LootRandomNum = 0;

    private float gameTime = 180.0f;

    public Text timerText1;
    public Text timerText2;

    public float seconds;
    public float minutes;

    private bool gameOver;

    private float endTime = 0.0f;

    public SceneManage sceneManage;
    // Start is called before the first frame update
    void Start()
    {
        LootSpawnList = GameObject.FindGameObjectsWithTag("LootSpawn");

        gameOver = false;

        //Initialize UI elements
        timerText1 = timerText1.GetComponent<Text>();
        timerText2 = timerText2.GetComponent<Text>();
    }

    private void Update()
    {
		//For quitting to menu
		if (Input.GetKeyDown(KeyCode.M))
		{
			sceneManage.GoToFirstScene();
		}
		//Update game time
		gameTime -= Time.deltaTime;

        //Display Time
        minutes = (int)(gameTime / 60f);
        seconds = (int)(gameTime % 60f);

        if (!gameOver)
        {
            timerText1.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            timerText2.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }     


        //End Conditions
        if (gameTime <= 0.0f)
        {
            //timerText1.text = "00" + ":" + "00";
            //timerText2.text = "00" + ":" + "00";

            if (!gameOver)
            {
                endTime = gameTime;
                gameOver = true;
            }

            if (gameTime < endTime - 3.0f)
            {           
                sceneManage.GoToFirstScene();
            }
        }

        if (GameObject.FindGameObjectsWithTag("Loot").Length < 6)
        {
            SpawnNewDodgeball();
        }
    }

    private void SpawnNewDodgeball()
    {
        LootRandomNum = Random.Range(0, LootSpawnList.Length);
        if (LootSpawnList[LootRandomNum].GetComponent<LootSpawnManager>().hasDodgeball == false)
        {
            GameObject DodgeballLootInstance = Instantiate(DodgeballLootPrefab, RandomPointInBounds(LootSpawnList[LootRandomNum].GetComponent<BoxCollider>().bounds), Quaternion.identity);
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
