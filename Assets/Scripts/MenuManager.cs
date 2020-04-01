using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image instructions;
    public Button yaReady;
    public Button FirstBtn;
    public EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        instructions = instructions.GetComponent<Image>();
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(FirstBtn.gameObject);
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
