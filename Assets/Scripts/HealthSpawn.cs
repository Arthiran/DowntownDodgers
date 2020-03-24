using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour
{
    public GameObject healthPrefab;
    private GameObject[] healthSpawnList;
    private Target target;
    private GameManagerScript gm;
    private Target[] allTargets;
    private float totalHealth;
    private float averageNum;
    private int randomSpawn;
    public bool healthSpawned = false;
    public float timeToSpawnHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthSpawnList = GameObject.FindGameObjectsWithTag("HealthSpawn");
        allTargets = GameObject.FindObjectsOfType<Target>();
        target = GameObject.FindObjectOfType<Target>();
        gm = GameObject.FindObjectOfType<GameManagerScript>();

        //Calculation to check average player health
        averageNum = (target.originalHealth * gm.NumberOfPlayers) * 0.67f;
        //Debug.Log(averageNum);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < allTargets.Length; i++)
        {
            totalHealth += allTargets[i].health;
        }

        /*if (totalHealth <= averageNum)
        {
        }*/
        if (!healthSpawned && timeToSpawnHealth >= 30.0f)
        {
            healthSpawned = true;
            timeToSpawnHealth = 0.0f;
            SpawnHealth();
        }

        //Reset total health checker
        totalHealth = 0.0f;
        
        if (!healthSpawned)
        {
            timeToSpawnHealth += Time.deltaTime;
            Debug.Log(timeToSpawnHealth);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnHealth();
        }
    }

    private void SpawnHealth()
    {
        Debug.Log("HEALTH");
        randomSpawn = Random.Range(0, healthSpawnList.Length);

        GameObject healthInstance = Instantiate(healthPrefab, 
            healthSpawnList[randomSpawn].transform.position, Quaternion.identity);
    }
}
