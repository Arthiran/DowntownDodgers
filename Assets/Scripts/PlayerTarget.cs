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
        RaycastHit[] hits;
        hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 99999.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
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
    }
}
