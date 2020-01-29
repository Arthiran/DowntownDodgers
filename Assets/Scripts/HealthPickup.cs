using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    //Global Variables
    private float pickupTimer;

    // Start is called before the first frame update
    void Start()
    {
        pickupTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            pickupTimer = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Health")
        {
            if (Input.GetKey(KeyCode.E))
            {
                pickupTimer += Time.deltaTime;

                if (pickupTimer >= 4.0f)
                {
                    if (gameObject.GetComponent<Target>().health < 30.0f)
                    {
                        //Add Health
                        gameObject.GetComponent<Target>().health += 10.0f;

                        //Update UI
                        gameObject.GetComponent<Target>().MovementController.HealthNumText.text = 
                            gameObject.GetComponent<Target>().health.ToString() + "/" + gameObject.GetComponent<Target>().originalHealth.ToString();

                        gameObject.GetComponent<Target>().MovementController.filledHealthbarIMG.fillAmount = 
                            gameObject.GetComponent<Target>().health / gameObject.GetComponent<Target>().originalHealth;

                        //Destroy the pickup
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}
