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
    [HideInInspector]
    public int playerCount = 4;
    public GameObject PlayerPrefab;
    private GameObject[] LootSpawnList;
    //private GameObject[] PlayerList;

    public GameObject DodgeballLootPrefab;
    public GameObject DodgeballPrefab;
    public BoxCollider rainingballsBounds;

    private int LootRandomNum = 0;

    private float gameTime = 90.0f;

    public Text timerText1;
    public Text timerText2;
    public Text timerText3;
    public Text timerText4;

    [HideInInspector]
    public bool isCountingDown = true;
    public Text countDownText1;
    public Text countDownText2;
    public Text countDownText3;
    public Text countDownText4;

    public Image timerCircle1;
    public Image timerCircle2;
    public Image timerCircle3;
    public Image timerCircle4;

    public float seconds;
    public float minutes;

    private bool gameOver;
    public bool gameStart;

    private float endTime = 0.0f;

    public SceneManage sceneManage;
    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < NumberOfPlayers; i++)
        {
            PlayerList[i] = GameObject.Find("Player" + i + 1);
            PlayerList[i].GetComponentInChildren<PlayerMovementController>().enabled = false;
        }*/
        gameStart = false;
        countDownText1 = countDownText1.GetComponent<Text>();
        countDownText2 = countDownText2.GetComponent<Text>();
        countDownText3 = countDownText3.GetComponent<Text>();
        countDownText4 = countDownText4.GetComponent<Text>();
        StartCoroutine(Countdown(3));

        LootSpawnList = GameObject.FindGameObjectsWithTag("LootSpawn");

        gameOver = false;

        //Initialize UI elements
        timerText1 = timerText1.GetComponent<Text>();
        timerText2 = timerText2.GetComponent<Text>();
        timerText3 = timerText3.GetComponent<Text>();
        timerText4 = timerText4.GetComponent<Text>();

        timerText1.text = "";
        timerText2.text = "";
        timerText3.text = "";
        timerText4.text = "";

        timerCircle1 = timerCircle1.GetComponent<Image>();
        timerCircle2 = timerCircle2.GetComponent<Image>();
        timerCircle3 = timerCircle3.GetComponent<Image>();
        timerCircle4 = timerCircle4.GetComponent<Image>();
        playerCount = FindObjectsOfType<PlayerRootInfo>().Length;

        timerCircle1.enabled = false;
        timerCircle2.enabled = false;
        timerCircle3.enabled = false;
        timerCircle4.enabled = false;
    }

    IEnumerator Countdown(int seconds)
    {
        isCountingDown = true;
        int count = seconds;

        yield return new WaitForSeconds(2);

        countDownText1.fontSize = 75;
        countDownText2.fontSize = 75;
        countDownText3.fontSize = 75;
        countDownText4.fontSize = 75;

        while (count > 0)
        {
            //PlayerPrefab.GetComponent<PlayerMovementController>().enabled = false;
            countDownText1.text = count.ToString();
            countDownText2.text = count.ToString();
            countDownText3.text = count.ToString();
            countDownText4.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
            if (count == 0)
            {
                /*for (int i = 0; i < NumberOfPlayers; i++)
                {
                    PlayerList[i].GetComponentInChildren<PlayerMovementController>().enabled = true;
                }*/
                gameStart = true;
                countDownText1.text = "GO!";
                countDownText2.text = "GO!";
                countDownText3.text = "GO!";
                countDownText4.text = "GO!";
                yield return new WaitForSeconds(1);
                countDownText1.enabled = false;
                countDownText2.enabled = false;
                countDownText3.enabled = false;
                countDownText4.enabled = false;
            }
        }
        isCountingDown = false;
    }

    private void Update()
    {
        playerCount = FindObjectsOfType<PlayerMovementController>().Length;
        //For quitting to menu
        if (Input.GetKeyDown(KeyCode.M))
		{
			sceneManage.GoToFirstScene();
		}

        if (gameStart && playerCount == 2)
        {
            timerCircle1.enabled = true;
            timerCircle2.enabled = true;
            timerCircle3.enabled = true;
            timerCircle4.enabled = true;

            //Update game time
            gameTime -= Time.deltaTime;

            //Display Time
            minutes = (int)(gameTime / 60f);
            seconds = (int)(gameTime % 60f);

            timerCircle1.fillAmount -= Time.deltaTime / 90.0f; 
            timerCircle2.fillAmount -= Time.deltaTime / 90.0f; 
            timerCircle3.fillAmount -= Time.deltaTime / 90.0f; 
            timerCircle4.fillAmount -= Time.deltaTime / 90.0f; 

            if (!gameOver)
            {
                timerText1.text = minutes.ToString("0") + ":" + seconds.ToString("00");
                timerText2.text = minutes.ToString("0") + ":" + seconds.ToString("00");
                timerText3.text = minutes.ToString("0") + ":" + seconds.ToString("00");
                timerText4.text = minutes.ToString("0") + ":" + seconds.ToString("00");
            }


            //End Conditions
            if (gameTime <= 0.0f)
            {
                //timerText1.text = "00" + ":" + "00";
                //timerText2.text = "00" + ":" + "00";
                RainingBalls();
                if (!gameOver)
                {
                    endTime = gameTime;
                    gameOver = true;
                }

                /*if (gameTime < endTime - 3.0f)
                {
                    sceneManage.GoToFirstScene();
                }*/
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

                /*if (gameTime < endTime - 3.0f)
                {
                    sceneManage.GoToFirstScene();
                }*/
            }
        }

        if (playerCount <= 1)
        {
            StartCoroutine(GoToMenu());
        }

        if (GameObject.FindGameObjectsWithTag("Loot").Length < 32)
        {
            SpawnNewDodgeball();
        }
    }

    private IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(3.0f);
        sceneManage.GoToFirstScene();
    }

    private void SpawnNewDodgeball()
    {
        LootRandomNum = Random.Range(0, LootSpawnList.Length);
        if (LootSpawnList[LootRandomNum].GetComponent<LootSpawnManager>().hasDodgeball == false)
        {
            GameObject DodgeballLootInstance = Instantiate(DodgeballLootPrefab, RandomPointInBounds(LootSpawnList[LootRandomNum].GetComponent<BoxCollider>().bounds), Quaternion.identity);
        }
    }

    private void RainingBalls()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(DodgeballPrefab, RandomPointInBounds(rainingballsBounds.bounds), Quaternion.identity);
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
