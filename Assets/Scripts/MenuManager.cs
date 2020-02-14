using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image instructions;
    public Button yaReady;

    // Start is called before the first frame update
    void Start()
    {
        instructions = instructions.GetComponent<Image>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
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
