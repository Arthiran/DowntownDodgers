using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelEditorScript : MonoBehaviour
{
    public enum SpawnableSet { SpawnableSet1, SpawnableSet2, SpawnableSet3 }
    public SpawnableSet spawnableSet = SpawnableSet.SpawnableSet1;

    private const string DLL_NAME = "LevelEditorPlugin";
    public Camera CurrentCamera;
    private List<GameObject> spawnables = new List<GameObject>();
    public List<GameObject> SpawnableSet1 = new List<GameObject>();
    public List<GameObject> SpawnableSet2 = new List<GameObject>();
    public List<GameObject> SpawnableSet3 = new List<GameObject>();
    private List<GameObject> spawnablePrefabs = new List<GameObject>();

    private bool finishedLoading = false;
    private bool isSelected = false;
    private bool isCarrying = false;
    private int readParameters = 10;
    private GameObject objecttobeSpawned;
    private Transform selectedObject;
    public GameObject TransformBox;

    private EditorCameraScript editorScript;

    //Location UI
    public InputField LocationXInput;
    public InputField LocationYInput;
    public InputField LocationZInput;

    //Rotation UI
    public InputField RotationXInput;
    public InputField RotationYInput;
    public InputField RotationZInput;

    //Scale UI
    public InputField ScaleXInput;
    public InputField ScaleYInput;
    public InputField ScaleZInput;

    //Load and Save UI
    public GameObject LoadandSaveBox;
    public InputField SaveInputField;
    public InputField LoadInputField;

    private string folderLocation = Application.streamingAssetsPath + "\\";
    private string defaultInputText = "default";
    private const string textExtension = ".txt";
    private string fileToSave;

    private string previousLevelLoaded = "";

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
        fileToSave = folderLocation + defaultInputText + textExtension;
        if (spawnableSet.ToString() == "SpawnableSet1")
        {
            spawnablePrefabs = SpawnableSet1;
        }
        else if (spawnableSet.ToString() == "SpawnableSet2")
        {
            spawnablePrefabs = SpawnableSet2;
        }
        else if (spawnableSet.ToString() == "SpawnableSet3")
        {
            spawnablePrefabs = SpawnableSet3;
        }

        TransformBox.SetActive(false);
        LoadandSaveBox.SetActive(false);
        SaveInputField.text = defaultInputText;
        LoadInputField.text = defaultInputText;
    }

    // Update is called once per frame
    void Update()
    {
        checkObjectEditable();
        deleteObject();

        if (Input.GetKeyDown(KeyCode.X))
        {
            TransformBox.SetActive(false);
            LoadandSaveBox.SetActive(false);
            if (CurrentCamera.name != "Camera1")
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (isSelected == true)
        {
            EditTransform(selectedObject);
        }

        if (isCarrying == true)
        {
            objectSpawner(objecttobeSpawned);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (LoadandSaveBox.activeSelf)
            {
                LoadandSaveBox.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (LoadandSaveBox.activeSelf == false)
            {
                LoadandSaveBox.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            clearLevel();
        }
    }

    public void saveObjects()
    {
        if (SaveInputField.text != "")
        {
            fileToSave = folderLocation + SaveInputField.text + textExtension;
            Spawnable[] spawnablestemp = FindObjectsOfType<Spawnable>();
            GameObject[] spawnableGameObjects = new GameObject[spawnablestemp.Length];
            if (spawnablestemp.Length > 0)
            {
                for (int i = 0; i < spawnablestemp.Length; i++)
                {
                    spawnableGameObjects[i] = spawnablestemp[i].gameObject;
                }
            }
            spawnables.AddRange(spawnableGameObjects);
            startWriting(fileToSave);
            for (int i = 0; i < spawnablePrefabs.Count; i++)
            {
                for (int j = 0; j < spawnables.Count; j++)
                {
                    if (spawnablePrefabs[i].name.ToLower() == spawnables[j].name.Substring(0, spawnables[j].gameObject.name.Length - 7).ToLower() || 
                        spawnablePrefabs[i].name.ToLower() == spawnables[j].name.ToLower())
                    {
                        ReadInto(i, spawnables[j].transform.position.x, spawnables[j].transform.position.y, spawnables[j].transform.position.z,
                            spawnables[j].transform.eulerAngles.x, spawnables[j].transform.eulerAngles.y, spawnables[j].transform.eulerAngles.z,
                            spawnables[j].transform.localScale.x, spawnables[j].transform.localScale.y, spawnables[j].transform.localScale.z);
                    }
                }
            }
            endWriting();
        }
    }

    public void loadObjects()
    {
        if (previousLevelLoaded != LoadInputField.text)
        {
            previousLevelLoaded = LoadInputField.text;
            fileToSave = folderLocation + LoadInputField.text + textExtension;
            finishedLoading = true;
            int objectSet = 0;

            if (LoadInputField.text != "" && returnLines(fileToSave) != 0)
            {
                while (finishedLoading)
                {
                    for (int i = 0; i <= (returnLines(fileToSave) / readParameters); i++)
                    {
                        GameObject tempSpawnableObject;
                        tempSpawnableObject = Instantiate(spawnablePrefabs[Mathf.RoundToInt(ReadOut(0 + objectSet, fileToSave))], new Vector3(ReadOut(1 + objectSet, fileToSave), 
                            ReadOut(2 + objectSet, fileToSave), ReadOut(3 + objectSet, fileToSave)), Quaternion.Euler(ReadOut(4 + objectSet, fileToSave), ReadOut(5 + objectSet, fileToSave), 
                            ReadOut(6 + objectSet, fileToSave)));
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
    }

    public void objectButtonSpawn(int prefabNum)
    {
        if (isCarrying == false)
        {
            objecttobeSpawned = spawnablePrefabs[prefabNum];
            isCarrying = true;
        }
    }

    private void objectSpawner(GameObject _gameObject)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            Ray raycast;
            raycast = CurrentCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;


            //Object Instantiate
            if (Physics.Raycast(raycast, out hit, Mathf.Infinity))
            {
                Instantiate(_gameObject, new Vector3(hit.point.x, _gameObject.transform.position.y, hit.point.z), Quaternion.identity);
                isCarrying = false;
            }
        }
    }

    private void checkObjectEditable()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            Ray raycast;
            raycast = CurrentCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(raycast, out hit, 100.0f))
            {
                if (hit.transform.gameObject.GetComponent<Spawnable>())
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    selectedObject = hit.transform;
                    TransformBox.SetActive(true);
                    isSelected = true;
                }
                else
                {
                    if (CurrentCamera.name != "Camera1")
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                    TransformBox.SetActive(false);
                    isSelected = false;
                }
            }
        }
    }

    private void deleteObject()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            Ray raycast;
            raycast = CurrentCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(raycast, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<Spawnable>())
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    public void clearLevel()
    {
        Spawnable[] spawnablestemp = FindObjectsOfType<Spawnable>();
        for (int i = 0; i < spawnablestemp.Length; i++)
        {
            Destroy(spawnablestemp[i].gameObject);
        }
    }

    private void EditTransform(Transform _selectedObject)
    {
        TransformControls();
        //Location Stuff
        if (!LocationXInput.isFocused && !LocationYInput.isFocused && !LocationZInput.isFocused)
        {
            LocationXInput.text = _selectedObject.transform.position.x.ToString();
            LocationYInput.text = _selectedObject.transform.position.y.ToString();
            LocationZInput.text = _selectedObject.transform.position.z.ToString();
        }

        LocationXInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        LocationYInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        LocationZInput.onEndEdit.AddListener(delegate { UpdateTransform(); });


        //Rotation Stuff
        if (!RotationXInput.isFocused && !RotationYInput.isFocused && !RotationZInput.isFocused)
        {
            RotationXInput.text = _selectedObject.transform.eulerAngles.x.ToString();
            RotationYInput.text = _selectedObject.transform.eulerAngles.y.ToString();
            RotationZInput.text = _selectedObject.transform.eulerAngles.z.ToString();
        }

        RotationXInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        RotationYInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        RotationZInput.onEndEdit.AddListener(delegate { UpdateTransform(); });

        //Scale Stuff
        if (!ScaleXInput.isFocused && !ScaleYInput.isFocused && !ScaleZInput.isFocused)
        {
            ScaleXInput.text = _selectedObject.transform.localScale.x.ToString();
            ScaleYInput.text = _selectedObject.transform.localScale.y.ToString();
            ScaleZInput.text = _selectedObject.transform.localScale.z.ToString();
        }

        ScaleXInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        ScaleYInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
        ScaleZInput.onEndEdit.AddListener(delegate { UpdateTransform(); });
    }

    private void UpdateTransform()
    {
        selectedObject.transform.position = new Vector3(float.Parse(LocationXInput.text), float.Parse(LocationYInput.text), float.Parse(LocationZInput.text));
        selectedObject.transform.eulerAngles = new Vector3(float.Parse(RotationXInput.text), float.Parse(RotationYInput.text), float.Parse(RotationZInput.text));
        selectedObject.transform.localScale = new Vector3(float.Parse(ScaleXInput.text), float.Parse(ScaleYInput.text), float.Parse(ScaleZInput.text));
    }

    private void TransformControls()
    {
        float transformMovement;
        float transformMoveSpeed = 0.05f;
        float rotationMoveSpeed = 0.5f;
        if (Input.GetAxisRaw("TransformVertical") != 0)
        {
            transformMovement = Input.GetAxisRaw("TransformVertical") * transformMoveSpeed;
            selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, selectedObject.transform.position.z + transformMovement);
        }
        else if (Input.GetAxisRaw("TransformHorizontal") != 0)
        {
            transformMovement = Input.GetAxisRaw("TransformHorizontal") * transformMoveSpeed;
            selectedObject.transform.position = new Vector3(selectedObject.transform.position.x + transformMovement, selectedObject.transform.position.y, selectedObject.transform.position.z);
        }
        else if (Input.GetAxisRaw("TransformYAxis") != 0)
        {
            transformMovement = Input.GetAxisRaw("TransformYAxis") * transformMoveSpeed;
            selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y + transformMovement, selectedObject.transform.position.z);
        }
        else if (Input.GetAxisRaw("RotationXAxis") != 0)
        {
            transformMovement = Input.GetAxisRaw("RotationXAxis") * rotationMoveSpeed;
            selectedObject.transform.eulerAngles = new Vector3(selectedObject.transform.eulerAngles.x + transformMovement, selectedObject.transform.eulerAngles.y, selectedObject.transform.eulerAngles.z);
        }
        else if (Input.GetAxisRaw("RotationYAxis") != 0)
        {
            transformMovement = Input.GetAxisRaw("RotationYAxis") * rotationMoveSpeed;
            selectedObject.transform.eulerAngles = new Vector3(selectedObject.transform.eulerAngles.x, selectedObject.transform.eulerAngles.y + transformMovement, selectedObject.transform.eulerAngles.z);
        }
        else if (Input.GetAxisRaw("RotationZAxis") != 0)
        {
            transformMovement = Input.GetAxisRaw("RotationZAxis") * rotationMoveSpeed;
            selectedObject.transform.eulerAngles = new Vector3(selectedObject.transform.eulerAngles.x, selectedObject.transform.eulerAngles.y, selectedObject.transform.eulerAngles.z + transformMovement);
        }
    }
}
