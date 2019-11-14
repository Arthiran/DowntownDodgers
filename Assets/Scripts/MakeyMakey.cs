using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeyMakey : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            text.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            text.enabled = false;
        }
        //text.enabled = false;
    }
}
