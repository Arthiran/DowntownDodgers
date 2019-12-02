using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SceneInitializerScript : MonoBehaviour
{
    private int readParameters = 10;

    public Material skybox1, skybox2;

    public List<Light> lights = new List<Light>();

    public GameObject bulb;
    public Material bulbOff, bulbOn;

    public List<GameObject> spawnablePrefabs = new List<GameObject>();

    private const string DLL_NAME = "LevelEditorPlugin";

    private string folderLocation = Application.streamingAssetsPath + "/";
    [SerializeField]
    private string levelName = "default";
    private const string textExtension = ".txt";
    private string fileToSave;

    private bool finishedLoading = false;

    [DllImport(DLL_NAME)]
    private static extern void startWriting(string fileName);
    [DllImport(DLL_NAME)]
    private static extern void endWriting();
    [DllImport(DLL_NAME)]
    private static extern void ReadInto(float objectNumber, float locationx, float locationy, float locationz, float rotationx,
    float rotationy, float rotationz, float scalex, float scaley, float scalez);
    [DllImport(DLL_NAME)]
    private static extern float ReadOut(int j, string fileName);
    [DllImport(DLL_NAME)]
    private static extern int returnLines(string fileName);

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
        fileToSave = folderLocation + levelName + textExtension;
        loadObjects();
    }

    public void loadObjects()
    {
        finishedLoading = true;
        int objectSet = 0;

        while (finishedLoading)
        {
            for (int i = 0; i <= (returnLines(fileToSave) / readParameters); i++)
            {
                GameObject tempSpawnableObject;
                tempSpawnableObject = Instantiate(spawnablePrefabs[Mathf.RoundToInt(ReadOut(0 + objectSet, fileToSave))], new Vector3(ReadOut(1 + objectSet, fileToSave), ReadOut(2 + objectSet, fileToSave), ReadOut(3 + objectSet, fileToSave)), Quaternion.Euler(ReadOut(4 + objectSet, fileToSave), ReadOut(5 + objectSet, fileToSave), ReadOut(6 + objectSet, fileToSave)));
                tempSpawnableObject.transform.localScale = new Vector3(ReadOut(7 + objectSet, fileToSave), ReadOut(8 + objectSet, fileToSave), ReadOut(8 + objectSet, fileToSave));
                objectSet = objectSet + readParameters;
                if (objectSet == returnLines(fileToSave))
                {
                    finishedLoading = false;
                    break;
                }
            }
            finishedLoading = false;
        }
    }
}
