using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoCinematicMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            player.transform.Translate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.G))
        {
            player.transform.Translate(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.F))
        {
            player.transform.Translate(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.H))
        {
            player.transform.Translate(1, 0, 0);
        }
    }
}
