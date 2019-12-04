using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public Text score1;
    public Text score2;

    private void Start()
    {
        score1.text = PresistantManage.Instance.p1Wins.ToString();
        score2.text = PresistantManage.Instance.p2Wins.ToString();
    }

    public void GoToFirstScene()
    {
        SceneManager.LoadScene("MenuScene");
        PresistantManage.Instance.value++;
    }
    public void GoToSecondScene()
    {
        SceneManager.LoadScene("TestmapNoNetwork");
        PresistantManage.Instance.value++;
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
        PresistantManage.Instance.value++;
    }

    public void GoToScore()
    {
        SceneManager.LoadScene("ScoreScene");
        PresistantManage.Instance.value++;
    }
}
