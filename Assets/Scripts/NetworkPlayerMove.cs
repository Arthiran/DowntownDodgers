using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(0, 0, 1);
        }

    }
}
