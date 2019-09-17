using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootInfo : MonoBehaviour
{
    public int PlayerID;
    private Camera Camera;
    private GameObject GMScript;
    // Start is called before the first frame update
    void Start()
    {
        GMScript = GameObject.FindGameObjectWithTag("GameManager");
        Camera = GetComponentInChildren<Camera>();
        if (GMScript.GetComponent<GameManagerScript>().NumberOfPlayers == 1)
        {
            Camera.rect = new Rect(0f, 0f, 1f, 1f);
        }
        else if (GMScript.GetComponent<GameManagerScript>().NumberOfPlayers == 2)
        {
            if (PlayerID == 1)
            {
                Camera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            }
            else if (PlayerID == 2)
            {
                Camera.rect = new Rect(0f, 0f, 1f, 0.5f);
            }
        }
        else if (GMScript.GetComponent<GameManagerScript>().NumberOfPlayers == 3)
        {
            if (PlayerID == 1)
            {
                Camera.rect = new Rect(0f, 0.5f, 1, 0.5f);
            }
            else if (PlayerID == 2)
            {
                Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
            }
            else if (PlayerID == 3)
            {
                Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
            }
        }
        else if (GMScript.GetComponent<GameManagerScript>().NumberOfPlayers == 4)
        {
            if (PlayerID == 1)
            {
                Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
            }
            else if (PlayerID == 2)
            {
                Camera.rect = new Rect(0.5f, 0.5f,0.5f, 0.5f);
            }
            else if (PlayerID == 3)
            {
                Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
            }
            else if (PlayerID == 4)
            {
                Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
            }
        }
    }
}
