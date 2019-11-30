using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScriptNoNetwork : MonoBehaviour
{
    private GameObject PlayerInstance;
    [Range(1,4)]
    public int NumberOfPlayers = 1;
    public GameObject PlayerPrefab;
    private GameObject[] LootSpawnList;

    public GameObject DodgeballLootPrefab;

    private int LootRandomNum = 0;
    public int p1Score = 0;
    public int p2Score = 0;

    private float gameTime = 180.0f;

    public Text p1ResultText;
    public Text p2ResultText;

    public Text timerText1;
    public Text timerText2;

    public Text scoreText1;
    public Text scoreText2;

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

        //Set scores to start at 0
        p1Score = 0;
        p2Score = 0;

        //Initialize UI elements
        p1ResultText = p1ResultText.GetComponent<Text>();
        p2ResultText = p2ResultText.GetComponent<Text>();
        timerText1 = timerText1.GetComponent<Text>();
        timerText2 = timerText2.GetComponent<Text>();
        scoreText1 = scoreText1.GetComponent<Text>();
        scoreText2 = scoreText2.GetComponent<Text>();

        //Hide the text
        p1ResultText.enabled = false;
        p2ResultText.enabled = false;

        for (int i = 1; i <= 3; i++)
        {
            SpawnNewDodgeball();
        }
    }

    private void Update()
    {
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

        //Display Score
        scoreText1.text = p1Score.ToString("0");
        scoreText2.text = p2Score.ToString("0");


        //End Conditions
        if (gameTime <= 0.0f || p1Score == 3 || p2Score == 3)
        {
            //timerText1.text = "00" + ":" + "00";
            //timerText2.text = "00" + ":" + "00";

            if (!gameOver)
            {
                endTime = gameTime;
                gameOver = true;
            }

            //Make text visible
            p1ResultText.enabled = true;
            p2ResultText.enabled = true;

            //P1 wins
            if (p1Score > p2Score)
            {  
                p1ResultText.text = "You Win!";
                p2ResultText.text = "You Lose!";
            }
            //P2 wins
            else if (p2Score > p1Score)
            {
                p1ResultText.text = "You Lose!";
                p2ResultText.text = "You Win!";
            }
            //Tie game
            else
            {
                p1ResultText.text = "Tie Game!";
                p2ResultText.text = "Tie Game!";
            }

            if (gameTime < endTime - 3.0f)
            {           
                sceneManage.GoToFirstScene();
            }
        }

        if (GameObject.FindGameObjectsWithTag("Loot").Length < 3)
        {
            SpawnNewDodgeball();
        }
    }

    private void SpawnNewDodgeball()
    {
        LootRandomNum = Random.Range(0, LootSpawnList.Length - 1);
        GameObject DodgeballLootInstance = Instantiate(DodgeballLootPrefab, RandomPointInBounds(LootSpawnList[LootRandomNum].GetComponent<BoxCollider>().bounds), Quaternion.identity);
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
