using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class GameManagerScriptNoNetwork : MonoBehaviour
{
    private GameObject PlayerInstance;
    [Range(1,4)]
    public int NumberOfPlayers = 1;
    public GameObject PlayerPrefab;
    private GameObject[] SpawnPointList;

    public GameObject DodgeballLootPrefab;
    public Collider TempSpawnBox;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPointList = GameObject.FindGameObjectsWithTag("PlayerSpawn");

        /*for (int i = 0; i <= NumberOfPlayers - 1; i++)
        {
            PlayerInstance = Instantiate(PlayerPrefab, SpawnPointList[i].transform.position, Quaternion.Euler(SpawnPointList[i].transform.eulerAngles));
            PlayerInstance.GetComponent<PlayerRootInfo>().PlayerID = i + 1;
        }*/

        //CreatePlayer();

        for (int i = 1; i <= 3; i++)
        {
            SpawnNewDodgeball();
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Loot").Length < 3)
        {
            SpawnNewDodgeball();
        }
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerRoot"), SpawnPointList[0].transform.position, Quaternion.Euler(SpawnPointList[0].transform.eulerAngles));
    }

    private void SpawnNewDodgeball()
    {
        GameObject DodgeballLootInstance = Instantiate(DodgeballLootPrefab, RandomPointInBounds(TempSpawnBox.bounds), Quaternion.identity);
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
