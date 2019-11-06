using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Text toThrow;
    public Text toClimb;
    public Image wSpacebar;

    // Start is called before the first frame update
    void Start()
    {
        toThrow = GameObject.Find("Throw").GetComponent<Text>() ;
        toClimb = GameObject.Find("Climb").GetComponent<Text>() ;
        wSpacebar = GameObject.Find("WSpace").GetComponent<Image>() ;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }

        if (other.gameObject.name == "Trigger2")
        {
            //Reveal new instructions
            toClimb.gameObject.SetActive(true);
            wSpacebar.gameObject.SetActive(true);

            //GameObject.Find("Throw").SetActive(false);
            toThrow.gameObject.SetActive(false);
        }

    }
}
