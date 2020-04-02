using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image instructions;
    public GameObject selectedObject;
    public Button yaReady;
    public GameObject FirstBtn;
    private GameObject previousBtn;
    public EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        instructions = instructions.GetComponent<Image>();
        es.SetSelectedGameObject(FirstBtn.gameObject);
        selectedObject.transform.position = es.currentSelectedGameObject.gameObject.transform.position;
        previousBtn = FirstBtn;
    }

    private void Update()
    {
        if (es.currentSelectedGameObject != null)
        {
            selectedObject.transform.position = es.currentSelectedGameObject.gameObject.transform.position;
            previousBtn = es.currentSelectedGameObject.gameObject;
        }
        else
        {
            es.SetSelectedGameObject(previousBtn);
            selectedObject.transform.position = es.currentSelectedGameObject.gameObject.transform.position;
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void PlaySelected()
    {
        es.SetSelectedGameObject(yaReady.gameObject);
    }

    public void DisplayInstructions()
    {
        instructions.enabled = true;
        yaReady.image.enabled = true;
    }

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
