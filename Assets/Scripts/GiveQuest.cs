using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GiveQuest : MonoBehaviour
{
    public string currQuest;
    //public PlayerMovementControllerNoNetwork player;
    public GameObject moveQuest;
    public GameObject throwQuest;
    public GameObject climbQuest;
    public GameObject finishQuest;

    public Text questText;
    
    public void loadQuest(int x)
    {
        if(x == 1)
        {
            moveQuest.SetActive(true);
            questText.text = "THROW";
            
        }
        if (x == 2)
        {
           throwQuest.SetActive(true);
            questText.text = "CLIMB";
        }
        if (x == 3)
        {
           climbQuest.SetActive(true);
            questText.text = "FINISH";
        }
        if (x == 4)
        {
            finishQuest.SetActive(true);

        }
    }
}
