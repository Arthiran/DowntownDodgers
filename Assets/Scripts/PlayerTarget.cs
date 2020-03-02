using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTarget : MonoBehaviour
{
    public Image reticle;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        reticle = reticle.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            if (hit.transform.gameObject.name == "Player")
            {
                //Debug.Log("Hit");
                reticle.color = Color.red;
            }
            else
            {
                reticle.color = new Color(0.0f, 1.0f, 0.2f);
            }
        }
        else
        {
            reticle.color = new Color(0.0f, 1.0f, 0.2f);
        }
    }
}
