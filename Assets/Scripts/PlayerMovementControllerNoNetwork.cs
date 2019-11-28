using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovementControllerNoNetwork : MonoBehaviour
{
    public GiveQuest quest;
    //Get Character Properties
    private Transform Player;
    private CharacterController CharController;
    public Transform PlayerCam;
    public Image dashIconIMG;
    public Image dashCooldownIMG;
    public Image filledHealthbarIMG;
    public Text dashCooldownText;
    public Text interactText;
    public Text HealthNumText;
    public Text RespawningText;
    private ShootingNoNetwork shootingScript;
    private GameObject DodgeballInstance;
    private Target PlayerHealth;
    private GameObject[] SpawnPointList;

    public GameObject DodgeballPrefab;
    public Transform BallCarrierTransform;

    public GameObject sphere;

    private bool isDashCooldown;

    //Initialize Variables
    private float vertical;
    private float horizontal;
    private float LeftAnalogX;
    private float LeftAnalogY;
    private float velocityY;
    private Vector3 movement;
    private Vector3 currentImpact;
    public float moveSpeed = 5f;
    public float mass = 1f;
    public float damping = 5f;
    private float edgeUpForce = 10f;
    public float climbSpeed = 100f;
    public float jumpForce = 4f;
    public float dashForce = 4f;
    public float dashCooldown = 5f;
    private float dashCountdownUI;
    private float nextDash;
    private float gravity = Physics.gravity.y;
    private float distWall = 0.6f;
    public bool isGrounded = false;
    public bool isClimbing = false;
    private bool isFalling = false;
    private bool isRespawning = false;
    public bool inHand = true;
    private bool inHandStored = false;
    public float climbWallTimer = 1f;
    private float tempClimbTimer;
    private float respawnTimer = 1f;
    private float tempRespawnTimer;
    public int tempRandomNum;
    public int PlayerID;

    private float forwardMovement;
    private float horizontalMovement;

    private int doubleJumpCheck = 0;

    //ControllerStrings
    private string LeftAnalogXString;
    private string LeftAnalogYString;
    private string ControllerJumpString;
    private string ControllerDashString;
    private string ControllerLoadString;
    private string ControllerInteractString;

    private void Start()
    {
        PlayerID = GetComponentInParent<PlayerRootInfo>().PlayerID;

        if (PlayerID == 1)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (PlayerID == 2)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (PlayerID == 3)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }

        //Set Controller Strings
        LeftAnalogXString = "LeftAnalogX" + PlayerID.ToString();
        LeftAnalogYString = "LeftAnalogY" + PlayerID.ToString();
        ControllerJumpString = "ControllerJump" + PlayerID.ToString();
        ControllerDashString = "ControllerDash" + PlayerID.ToString();
        ControllerLoadString = "ControllerLoad" + PlayerID.ToString();
        ControllerInteractString = "ControllerInteract" + PlayerID.ToString();
        SpawnPointList = GameObject.FindGameObjectsWithTag("PlayerSpawn");

        //Set Components to Variables at Start of Script
        Player = GetComponent<Transform>();
        CharController = GetComponent<CharacterController>();
        shootingScript = GetComponentInParent<ShootingNoNetwork>();
        PlayerHealth = GetComponent<Target>();

        tempClimbTimer = climbWallTimer;
        tempRespawnTimer = respawnTimer;

        dashCountdownUI = dashCooldown + 1;

        sphere.SetActive(false);
    }

    //Any Input(Keyboard or Mouse) should be in Update function
    private void Update()
    {
        //Checks if Player is on the ground, if true set Y Velocity to 0
        if (CharController.isGrounded && velocityY < 0f)
        {
            doubleJumpCheck = 0;
            isClimbing = false;
            isFalling = false;
            //inHandStored = inHand;
            tempClimbTimer = climbWallTimer;
            tempRespawnTimer = respawnTimer;
            velocityY = 0f;   
        }

        //Get Input from Horizontal and Vertical Axis and store them in variables
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            vertical = Input.GetAxis("Vertical");
            forwardMovement = vertical;
        }
        else if (Input.GetAxis(LeftAnalogYString) != 0)
        {
            LeftAnalogY = Input.GetAxis(LeftAnalogYString);
            forwardMovement = LeftAnalogY;
        }
        else
        {
            forwardMovement = Input.GetAxis("Vertical"); ;
        }

        if (isClimbing == false)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                horizontal = Input.GetAxis("Horizontal");
                horizontalMovement = horizontal;
            }
            else if (Input.GetAxis(LeftAnalogXString) != 0)
            {
                LeftAnalogX = Input.GetAxis(LeftAnalogXString);
                horizontalMovement = LeftAnalogX;
            }
            else
            {
                horizontalMovement = Input.GetAxis("Horizontal");
            } 
            Dash();
        }

        /*This is so that when you press W and A at the same time for instance, the player doesn't become faster,
        it remains the same speed*/
        if (forwardMovement != 0 && horizontalMovement != 0)
        {
            forwardMovement *= 0.7071f;
            horizontalMovement *= 0.7071f;
        }

        //Jump pls
        Jump();
        ClimbWall();
        LoadDodgeball();

        if (isDashCooldown)
        {
            dashIconIMG.color = new Color(dashIconIMG.color.r, dashIconIMG.color.g, dashIconIMG.color.b, 1f);
            dashCooldownIMG.fillAmount += 1 / dashCooldown * Time.deltaTime;
            dashCountdownUI -= Time.deltaTime;
            if (dashCooldownIMG.fillAmount >= 1)
            {
                dashCountdownUI = 0;
                dashCooldownIMG.fillAmount = 0;
                dashCountdownUI = dashCooldown + 1;
                dashCooldownText.text = "";
                isDashCooldown = false;
            }
            else
            {
                dashIconIMG.color = new Color(dashIconIMG.color.r, dashIconIMG.color.g, dashIconIMG.color.b, 0.1f);
                dashCooldownText.text = dashCountdownUI.ToString();
            }
        }
    }

    //All Physics and Movement should be handled in FixedUpdate()
    private void FixedUpdate()
    {
        //Create a Vector to store the overall movement
        movement = new Vector3(horizontalMovement, 0, forwardMovement);
        Vector3 velocity;

        /*Takes the movement vector and converts the position from Local Space to World Space and stores 
        it back in the movement variable*/
        movement = transform.TransformDirection(movement);

        //Calculates gravity and stores it in variable
        velocityY += gravity * Time.deltaTime;

        //Vector which stores the overall effect of gravity on the Character's position
        if (inHand == true)
        {
            velocity = movement * moveSpeed + Vector3.up * velocityY;
        }
        else
        {
            velocity = movement * moveSpeed * 1.2f + Vector3.up * velocityY;
        }

        //sets the velocity to take all forces into account
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        if (DodgeballInstance != null)
        {
            //DodgeballInstance.transform.position = BallCarrierTransform.position;
        }

        //Takes the velocity and actually moves the Character
        if (isRespawning == false)
        {
            CharController.Move(velocity * Time.fixedDeltaTime);
        }

        isGrounded = CharController.isGrounded;

        //Interpolates the effects of forces for smooth movement
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    }

    //Resets all forces
    private void ResetImpact()
    {
        currentImpact = Vector3.zero;
        velocityY = 0f;
    }

    //Resets forces on the Y axis
    private void ResetImpactY()
    {
        currentImpact.y = 0f;
        velocityY = 0f;
    }

    //Jump Function
    private void Jump()
    {
        //Checks if Space was pressed
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown(ControllerJumpString))
        {
            doubleJumpCheck++;
            //Checks if player is on the ground, if true then character can jump
            if (doubleJumpCheck == 1 && isFalling == false && CharController.isGrounded == true)
            {
                //Custom AddForce function which applies jumpForce in the upward direction
                AddForce(Vector3.up, jumpForce);
            }
        }
    }

    private void Dash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetAxisRaw(ControllerDashString) > 0) && Time.time > nextDash)
        {
            isDashCooldown = true;
            nextDash = Time.time + dashCooldown;
            AddForce(PlayerCam.transform.forward, dashForce);
        }
    }

    private void LoadDodgeball()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown(ControllerLoadString))
        {
            if (shootingScript.DodgeballsInHand != 0 && inHand == false)
            {
                inHand = true;
                sphere.SetActive(true);
                //DodgeballInstance = Instantiate(DodgeballPrefab, BallCarrierTransform.position, BallCarrierTransform.rotation);
            }
            else if (shootingScript.DodgeballsInHand != 0 && inHand == true)
            {
                inHand = false;
                sphere.SetActive(false);
                inHandStored = inHand;
                //Destroy(DodgeballInstance);
            }
        }
        else if (shootingScript.DodgeballsInHand == 0)
        {
            if (inHand == false)
            {
                sphere.SetActive(false);
                if (DodgeballInstance != null)
                {
                    //Destroy(DodgeballInstance);
                }
            }
            else if (inHand == true)
            {
                if (inHandStored != true)
                {
                    sphere.SetActive(false);
                    inHandStored = inHand;
                    inHand = false;
                }
                inHand = false;
            }
        }
        else if (shootingScript.DodgeballsInHand != 0)
        {
            if (DodgeballInstance == null && inHandStored == true && isClimbing == false)
            {
                //DodgeballInstance = Instantiate(DodgeballPrefab, BallCarrierTransform.position, BallCarrierTransform.rotation);
                sphere.SetActive(true);
                inHand = true;
            }
            else if (DodgeballInstance != null && inHand == false)
            {
                sphere.SetActive(false);
                //Destroy(DodgeballInstance);
            }
        }
    }

    //Custom AddForce function, not to be mistakened with Rigidbody.AddForce()
    public void AddForce(Vector3 direction, float magnitude)
    {
        //Adds the force to the current amount of forces being applied to the game object
        currentImpact += direction.normalized * magnitude / mass;
    }

    private void ClimbWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distWall))
        {
            if (hit.collider.tag == "Climable")
            {
                if ((Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw(LeftAnalogYString) > 0) && (Input.GetButton("Jump") || Input.GetButton(ControllerJumpString)) && tempClimbTimer > 0)
                {
                    tempClimbTimer -= Time.deltaTime;
                    ResetImpactY();
                    //Gives the Character movement on the Y axis to climb up the wall
                    CharController.Move(new Vector3(0f, climbSpeed * Time.deltaTime, 0f));
                    isClimbing = true;
                    if (inHand == true)
                    {
                        sphere.SetActive(false);
                        inHandStored = inHand;
                        inHand = false;
                    }
                }
                else if (tempClimbTimer <= 0)
                {
                    isClimbing = false;
                    isFalling = true;
                }
                else
                {
                    isClimbing = false;
                }
            }
        }
        else if (isClimbing == true && isFalling == false)
        {
            //Resets Forces on the Y axis
            ResetImpactY();
            //This is to nudge the character above the ledge of the top of the wall
            AddForce(Vector3.up, edgeUpForce);
            tempClimbTimer = climbWallTimer;
            isClimbing = false;
            isFalling = false;
            if (inHandStored)
            {
                sphere.SetActive(true);
            }
        }
        else
        {
            tempClimbTimer = climbWallTimer;
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Loot")
        {
            if (interactText.color.a != 1 && shootingScript.DodgeballsInHand != shootingScript.DodgeballCarryLimit)
            {
                interactText.color = new Color(interactText.color.r, interactText.color.g, interactText.color.b, 1f);
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown(ControllerInteractString))
            {
                if (shootingScript.DodgeballsInHand < shootingScript.DodgeballCarryLimit)
                {
                    shootingScript.DodgeballsInHand++;
                    Destroy(collider.gameObject);
                }
                if (interactText.color.a != 0)
                {
                    interactText.color = new Color(interactText.color.r, interactText.color.g, interactText.color.b, 0f);
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Loot")
        {
            if (interactText.color.a != 0)
            {
                interactText.color = new Color(interactText.color.r, interactText.color.g, interactText.color.b, 0f);
            }
        }
    }

    public IEnumerator Respawn()
    {
        isRespawning = true;
        RespawningText.color = new Color(RespawningText.color.r, RespawningText.color.g, RespawningText.color.b, 1f);
        tempRandomNum = Random.Range(0, SpawnPointList.Length - 1);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(3);
        transform.position = SpawnPointList[tempRandomNum].transform.position;
        transform.eulerAngles = SpawnPointList[tempRandomNum].transform.eulerAngles;
        isRespawning = false;
        shootingScript.DodgeballsInHand = 0;
        nextDash = nextDash - dashCooldown;
        isDashCooldown = true;
        dashCooldownIMG.fillAmount = 1f;
        RespawningText.color = new Color(RespawningText.color.r, RespawningText.color.g, RespawningText.color.b, 0f);
        HealthNumText.text = PlayerHealth.originalHealth.ToString() + "/" + PlayerHealth.originalHealth.ToString();
        filledHealthbarIMG.fillAmount = PlayerHealth.originalHealth / PlayerHealth.originalHealth;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }


}
