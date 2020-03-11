using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void GoToFirstScene()
    {
        SceneManager.LoadScene("MenuScene");
        PresistantManage.Instance.value++;
    }
    public void GoToSecondScene()
    {
        SceneManager.LoadScene("Testmap");
        PresistantManage.Instance.value++;
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
        PresistantManage.Instance.value++;
    }
    public void GoToNetwork()
    {
        SceneManager.LoadScene("Networking");
        PresistantManage.Instance.value++;
    }
}
