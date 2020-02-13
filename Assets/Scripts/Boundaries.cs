using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boundaries : MonoBehaviour
{
    private bool OB; //OB means Out of Bounds
    public float killTimer = 5.0f;
    //private float flashTimer = 0.0f;

    public Text obText;
    public Text obTimeText;
    public Image vignette;

    // Start is called before the first frame update
    void Start()
    {
        vignette = vignette.GetComponent<Image>();
        //obText = GameObject.Find("OutOfBoundsText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OB)
        {
            killTimer -= Time.deltaTime;
            //flashTimer += Time.deltaTime;

            obText.enabled = true;
            obTimeText.enabled = true;

            vignette.canvasRenderer.SetAlpha(1.0f);
            vignette.enabled = true; 
            //Make text flash
            /*if (flashTimer < 1.0f)
                obText.enabled = true;
            else if (flashTimer >= 1.0f && flashTimer < 1.5f)
                obText.enabled = false;
            else if (flashTimer >= 1.5f)
                flashTimer = 0.0f;*/

            obText.text = "Return to Playing Field: \n";
            obTimeText.text = ((int)killTimer).ToString();

            //Debug.Log(killTimer);
            if (killTimer <= 0.0f)
            {
                Debug.Log("DIE");
                this.gameObject.GetComponent<Target>().Die();

                killTimer = 5.0f;
                OB = false;
            }
        }
        else
        {
            obText.enabled = false;
            obTimeText.enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            killTimer = 5.0f;
            //flashTimer = 0.0f;
            OB = true;
            Debug.Log("DQ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            OB = false;
            //flashTimer = 0.0f;
            Debug.Log("NO DQ");
        }
    }


}
