using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SceneInitializerScript : MonoBehaviour
{
    public Material skybox1, skybox2;

    public List<Light> lights = new List<Light>();

    public GameObject bulb;
    public Material bulbOff, bulbOn;

    private void Start()
    {
        if (Random.value < 0.5)
        {
            RenderSettings.skybox = skybox1;
            foreach (var light in lights)
                light.enabled = false;
            bulb.transform.Find("default").GetComponent<MeshRenderer>().material = bulbOff;
        }
        else
        {
            RenderSettings.skybox = skybox2;
            foreach (var light in lights)
                light.enabled = true;
            bulb.transform.Find("default").GetComponent<MeshRenderer>().material = bulbOn;
        }
    }
}
