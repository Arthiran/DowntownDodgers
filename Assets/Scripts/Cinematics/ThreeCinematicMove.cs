using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeCinematicMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            player.transform.Translate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.K))
        {
            player.transform.Translate(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.J))
        {
            player.transform.Translate(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.L))
        {
            player.transform.Translate(1, 0, 0);
        }
    }
}
