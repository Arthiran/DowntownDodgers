using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class MetricsLogger : MonoBehaviour
{
    const string DLL_NAME = "Tutorial2";

    //Initialize the DLL functions
    [DllImport(DLL_NAME)]
    private static extern void SaveTime(float aTime);
    [DllImport(DLL_NAME)]
    private static extern float LoadTime();
    [DllImport(DLL_NAME)]
    private static extern float GetTime();

    //Delta time variable
    float timePassed = 0.0f;

    //Bool for determining if tutorial is finished
    bool finished = false;

    // Update is called once per frame
    void Update()
    {
        //Update the delta time variable
        timePassed = Time.time;

        //If the user presses the L key output the time it took to complete the tutorial from reading the file
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(LoadTime());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Trigger4")
        {
            //Only check the first time they pass through the last trigger
            if (!finished)
            {
                //Save the time it took to compelte the tutorial
                SaveTime(timePassed);
                //Level is finished
                finished = true;
            }
        }
    }
}
