using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour
{
    public GameObject healthPrefab;
    private GameObject[] healthSpawnList;
    private int randomSpawn;

    // Start is called before the first frame update
    void Start()
    {
        healthSpawnList = GameObject.FindGameObjectsWithTag("HealthSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnHealth();
        }
    }

    private void SpawnHealth()
    {
        randomSpawn = Random.Range(0, healthSpawnList.Length);

        GameObject healthInstance = Instantiate(healthPrefab, 
            healthSpawnList[randomSpawn].transform.position, Quaternion.identity);
    }
}
