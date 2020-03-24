using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPickup : MonoBehaviour
{
    //Global Variables
    private float pickupTimer;
    public Image reticle;
    public float timeToPickup = 4.0f;
    private HealthSpawn healthSpawn;
    private bool isPlaying = false;
    private FMODUnity.StudioEventEmitter[] eventEmitter;


    // Start is called before the first frame update
    void Start()
    {
        pickupTimer = 0.0f;

        reticle = reticle.GetComponent<Image>();

        healthSpawn = GameObject.FindObjectOfType<HealthSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            pickupTimer = 0.0f;
            reticle.fillAmount = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Health")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!isPlaying)
                {
                    eventEmitter[0].Play();
                    isPlaying = true;
                }

                pickupTimer += Time.deltaTime;

                reticle.fillAmount += Time.deltaTime / timeToPickup;

                if (pickupTimer >= timeToPickup)
                {
                    Debug.Log("Heel'd");
                    isPlaying = false;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Heal", GetComponent<Transform>().position);

                    if (gameObject.GetComponent<Target>().health < 30.0f)
                    {
                      

                        //Add Health
                        gameObject.GetComponent<Target>().health += 10.0f;

                        //Update UI
                        gameObject.GetComponent<Target>().MovementController.HealthNumText.text =
                            gameObject.GetComponent<Target>().health.ToString() + "/" + gameObject.GetComponent<Target>().originalHealth.ToString();

                        gameObject.GetComponent<Target>().MovementController.filledHealthbarIMG.fillAmount =
                            gameObject.GetComponent<Target>().health / gameObject.GetComponent<Target>().originalHealth;

                    }

                    reticle.fillAmount = 0.0f;

                    //Destroy the pickup
                    Destroy(other.gameObject);

                    healthSpawn.healthSpawned = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Reset pickup action
        if (other.tag == "Health")
        {
            reticle.fillAmount = 0.0f;
            pickupTimer = 0.0f;
        }
    }
}
