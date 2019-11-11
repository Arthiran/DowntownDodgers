using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    public Text toThrow;
    public Text toClimb;
    public Text theEnd;
    public Image wSpacebar;

    public float timePassed;

    public ObjectPool pool;

    private bool end = false;

    private int counter = 0;

    public Quest quest;
    public GiveQuest giveQuest;
   
    // Start is called before the first frame update
    void Start()
    {
        
        toThrow = GameObject.Find("Throw").GetComponent<Text>();
        toClimb = GameObject.Find("Climb").GetComponent<Text>();
        theEnd = GameObject.Find("End").GetComponent<Text>();
        wSpacebar = GameObject.Find("WSpace").GetComponent<Image>();

        timePassed = Time.time;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            //yield return new WaitForSeconds(0.2f);
            if ((Time.time - timePassed) > 1.0f)
            {

                //Debug.Log(counter);

                GameObject theBall;

                theBall = pool.SpawnBall();

                theBall.transform.position = GameObject.Find("Object Pool").transform.position;


                counter++;

                if (counter >= 8)
                {
                    pool.DespawnBall(theBall);
                }

                timePassed = Time.time;
            }
            //counter++;

            //if (counter > 5)
            //{
            //pool.DespawnBall(theBall);
            //counter = 0;
            //}


            //yield return new WaitForSeconds(1.0f);
            //CheckLife(theBall);
        }
    }

    IEnumerator CheckLife(GameObject temp)
    {
        yield return new WaitForSeconds(2.0f);
        pool.DespawnBall(temp);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Trigger1")
        {
            //Hide the old instructions
            GameObject.Find("WASD").SetActive(false);
            GameObject.Find("Mouse").SetActive(false);
            GameObject.Find("Spacebar").SetActive(false);
            GameObject.Find("Move").SetActive(false);

            //Reveal new instructions
            toThrow.gameObject.SetActive(true);

            giveQuest.loadQuest(1);
        }

        if (other.gameObject.name == "Trigger2")
        {
            //Reveal new instructions
            toClimb.gameObject.SetActive(true);
            wSpacebar.gameObject.SetActive(true);

            //Hide
            toThrow.gameObject.SetActive(false);

            giveQuest.loadQuest(2);
        }

        if (other.gameObject.name == "Trigger3")
        {
            //Hide
            wSpacebar.gameObject.SetActive(false);
            toClimb.gameObject.SetActive(false);

            giveQuest.loadQuest(3);
        }

        if (other.gameObject.name == "Trigger4")
        {
            //Hide
            theEnd.gameObject.SetActive(true);
            giveQuest.loadQuest(4);

            end = true;

        }
    }
  

}
