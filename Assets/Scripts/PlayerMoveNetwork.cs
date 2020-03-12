using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveNetwork : MonoBehaviour
{
    public GameObject player;
    public NetworkingManager NetworkingManager;

    string msg;
    // Start is called before the first frame update
    void Start()
    {
        NetworkingManager.StartClient();
        //NetworkingManager.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(1,0,0);
            msg = ("v;" + player.transform.position.x.ToString() + ";" + player.transform.position.z.ToString());
            NetworkingManager.sendCurrPos(msg);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(-1, 0, 0);
            msg = ("v;" + player.transform.position.x.ToString() + ";" + player.transform.position.z.ToString());
            NetworkingManager.sendCurrPos(msg);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(0, 0, -1);
            msg = ("v;" + player.transform.position.x.ToString() + ";" + player.transform.position.z.ToString());
            NetworkingManager.sendCurrPos(msg);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(0, 0, 1);
            msg = ("v;" + player.transform.position.x.ToString() + ";" + player.transform.position.z.ToString());
            NetworkingManager.sendCurrPos(msg);
        }

    }
}
