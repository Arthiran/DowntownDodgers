using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camera0;
    public Camera camera1;
    public Canvas editorCanvas;
    public GameObject ObjectsPanel;
    private LevelEditorScript editorScript;

    // Start is called before the first frame update
    void Start()
    {
        camera0.gameObject.SetActive(true);
        camera1.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        editorCanvas.worldCamera = camera0;
        ObjectsPanel.SetActive(false);
        editorScript = FindObjectOfType<LevelEditorScript>();
        editorScript.CurrentCamera = camera0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (camera0.gameObject.activeSelf)
            {
                camera0.gameObject.SetActive(false);
                camera1.gameObject.SetActive(true);
                editorCanvas.worldCamera = camera1;
                ObjectsPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                editorScript.CurrentCamera = camera1;
            }
            else if (camera1.gameObject.activeSelf)
            {
                camera1.gameObject.SetActive(false);
                camera0.gameObject.SetActive(true);
                editorCanvas.worldCamera = camera0;
                ObjectsPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                editorScript.CurrentCamera = camera0;
            }
        }
    }
}
