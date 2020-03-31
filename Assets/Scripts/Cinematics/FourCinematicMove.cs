using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourCinematicMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            player.transform.Translate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.X))
        {
            player.transform.Translate(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.C))
        {
            player.transform.Translate(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.V))
        {
            player.transform.Translate(1, 0, 0);
        }
    }
}
