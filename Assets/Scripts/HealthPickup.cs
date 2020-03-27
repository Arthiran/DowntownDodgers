using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthPickup : MonoBehaviour
{
    //Global Variables
    private float pickupTimer;
    public Image reticle;
    public Text interact;
    public float timeToPickup = 4.0f;
    private HealthSpawn healthSpawn;
    private bool isPlaying = false;
    private FMODUnity.StudioEventEmitter[] eventEmitter;
    [FMODUnity.EventRef] private string FmodBandaging = "event:/Bandaging";
    [FMODUnity.EventRef] private string FmodHeal = "event:/Heal";
    


    



    private int PlayerID;
    private string ControllerInteractString;


    // Start is called before the first frame update
    void Start()
    {
        PlayerID = GetComponentInParent<PlayerRootInfo>().PlayerID;

        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                //Set Controller Strings
                ControllerInteractString = "ControllerInteract1";
            }
            else
            {
                //Set Controller Strings
                ControllerInteractString = "ControllerInteract" + PlayerID.ToString();
            }
        }
        else
        {
            //Set Controller Strings
            ControllerInteractString = "ControllerInteract1";
        }
        pickupTimer = 0.0f;

        reticle = reticle.GetComponent<Image>();
        interact = interact.GetComponent<Text>();

        healthSpawn = GameObject.FindObjectOfType<HealthSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp(ControllerInteractString))
        {
            pickupTimer = 0.0f;
            reticle.fillAmount = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Health")
        {
            interact.enabled = true;
            if (Input.GetKey(KeyCode.E) || Input.GetButton(ControllerInteractString))
            {
                if (!isPlaying)
                {
                    
                    isPlaying = true;
                    FMODUnity.RuntimeManager.PlayOneShot(FmodBandaging, transform.position);
                    //eventEmitter[3].Play();

                }

                pickupTimer += Time.deltaTime;

                reticle.fillAmount += Time.deltaTime / timeToPickup;

                if (pickupTimer >= timeToPickup)
                {
                    Debug.Log("Heel'd");
                    FMODUnity.RuntimeManager.PlayOneShot(FmodHeal, transform.position);
                    isPlaying = false;

                    // eventEmitter[4].Play();




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
            interact.enabled = false;
            reticle.fillAmount = 0.0f;
            pickupTimer = 0.0f;
        }
    }
}
