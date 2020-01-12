using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovementControllerNoNetwork : MonoBehaviour
{
    public GiveQuest quest;
    //Get Character Properties
    private Transform Player;
    private Animator PlayerAnimator;
    private CharacterController CharController;
    public Transform PlayerCam;
    public Image dashIconIMG;
    public Image dashCooldownIMG;
    public Image filledHealthbarIMG;
    public Image hitmarkerIMG;
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

    public Material player1Mat;
    public Material player2Mat;

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
    private float edgeUpForce = 30f;
    public float climbSpeed = 0f;
    public float jumpForce = 4f;
    public float dashForce = 4f;
    public float dashCooldown = 5f;
    private float dashCountdownUI;
    private float nextDash;
    private float gravity = Physics.gravity.y;
    private float distWall = 0.7f;
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

    public float forwardMovement;
    public float horizontalMovement;

    private int doubleJumpCheck = 0;

    //ControllerStrings
    private string LeftAnalogXString;
    private string LeftAnalogYString;
    private string ControllerJumpString;
    private string ControllerDashString;
    private string ControllerBackwardDashString;
    private string ControllerLoadString;
    private string ControllerInteractString;

    //MakeyMakey Things
    public Image p1Vision;
    public Image p2Vision;

    private float visionTime1 = 0.0f;
    private float visionTime2 = 0.0f;

    private bool blockVision1 = false;
    private bool blockVision2 = false;

    private void Start()
    {
        PlayerID = GetComponentInParent<PlayerRootInfo>().PlayerID;

        if (PlayerID == 1)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material = player1Mat;
        }
        else if (PlayerID == 2)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material = player2Mat;
        }
        else if (PlayerID == 3)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.green;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.magenta;
        }

        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                //Set Controller Strings
                LeftAnalogXString = "LeftAnalogX1";
                LeftAnalogYString = "LeftAnalogY1";
                ControllerJumpString = "ControllerJump1";
                ControllerDashString = "ControllerDash1";
                ControllerBackwardDashString = "ControllerBackwardDash1";
                ControllerLoadString = "ControllerLoad1";
                ControllerInteractString = "ControllerInteract1";
                SpawnPointList = GameObject.FindGameObjectsWithTag("PlayerSpawn");
            }
            else
            {
                //Set Controller Strings
                LeftAnalogXString = "LeftAnalogX" + PlayerID.ToString();
                LeftAnalogYString = "LeftAnalogY" + PlayerID.ToString();
                ControllerJumpString = "ControllerJump" + PlayerID.ToString();
                ControllerDashString = "ControllerDash" + PlayerID.ToString();
                ControllerBackwardDashString = "ControllerBackwardDash" + PlayerID.ToString();
                ControllerLoadString = "ControllerLoad" + PlayerID.ToString();
                ControllerInteractString = "ControllerInteract" + PlayerID.ToString();
                SpawnPointList = GameObject.FindGameObjectsWithTag("PlayerSpawn");
            }
        }
        else
        {
            //Set Controller Strings
            LeftAnalogXString = "LeftAnalogX1";
            LeftAnalogYString = "LeftAnalogY1";
            ControllerJumpString = "ControllerJump1";
            ControllerDashString = "ControllerDash1";
            ControllerBackwardDashString = "ControllerBackwardDash1";
            ControllerLoadString = "ControllerLoad1";
            ControllerInteractString = "ControllerInteract1";
            SpawnPointList = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        }
        //Set Components to Variables at Start of Script
        Player = GetComponent<Transform>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        CharController = GetComponent<CharacterController>();
        shootingScript = GetComponentInParent<ShootingNoNetwork>();
        PlayerHealth = GetComponent<Target>();

        tempClimbTimer = climbWallTimer;
        tempRespawnTimer = respawnTimer;

        dashCountdownUI = dashCooldown + 1;

        sphere.SetActive(false);

        p1Vision = p1Vision.GetComponent<Image>();
        p2Vision = p2Vision.GetComponent<Image>();
    }

    //Any Input(Keyboard or Mouse) should be in Update function
    private void Update()
    {
        vision();
        //Checks if Player is on the ground, if true set Y Velocity to 0
        if (isGrounded && velocityY < 0f)
        {
            doubleJumpCheck = 0;
            isClimbing = false;
            PlayerAnimator.SetBool("ClimbWall", false);
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
				//Debug.Log(horizontalMovement);
            }
            else
            {
                horizontalMovement = Input.GetAxis("Horizontal");
            } 
            Dash();
            BackwardDash();
        }

        /*This is so that when you press W and A at the same time for instance, the player doesn't become faster,
        it remains the same speed*/
        if (forwardMovement != 0 && horizontalMovement != 0)
        {
            forwardMovement *= 0.7071f;
            horizontalMovement *= 0.7071f;
            //Debug.Log("Bruv");
        }

        //Animations 
        PlayerAnimator.SetFloat("Vertical", forwardMovement);
        PlayerAnimator.SetFloat("Horizontal", horizontalMovement);

        //Jump pls
        Jump();
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

        if (isGrounded)
        {
            PlayerAnimator.SetBool("Jump", false);
        }

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
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, out hit, distWall))
            {
                if (hit.collider.tag == "Climable")
                {
                    StartCoroutine(ClimbWall(hit.collider));
                }
            }
            else
            {
                doubleJumpCheck++;
                PlayerAnimator.SetBool("Jump", true);
                //Checks if player is on the ground, if true then character can jump
                if (doubleJumpCheck == 1 && isFalling == false && CharController.isGrounded == true)
                {
                    //Custom AddForce function which applies jumpForce in the upward direction
                    AddForce(Vector3.up, jumpForce);
                }
            }
        }
    }

    private void Dash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetAxisRaw(ControllerDashString) > 0) && Time.time > nextDash)
        {
            StartCoroutine(DashAnimation());
            isDashCooldown = true;
            nextDash = Time.time + dashCooldown;
            AddForce(transform.forward, dashForce);
        }
    }
    private void BackwardDash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetAxisRaw(ControllerBackwardDashString) > 0) && Time.time > nextDash)
        {
            StartCoroutine(DashAnimation());
            isDashCooldown = true;
            nextDash = Time.time + dashCooldown;
            AddForce(transform.forward, -dashForce);
        }
    }

    private IEnumerator DashAnimation()
    {
        PlayerAnimator.SetBool("Dash", true);
        yield return new WaitForSeconds(0.3f);
        PlayerAnimator.SetBool("Dash", false);
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

    private IEnumerator ClimbWall(Collider wallCollider)
    {
        while ((Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw(LeftAnalogYString) > 0) && (Input.GetButton("Jump") || Input.GetButton(ControllerJumpString)) && tempClimbTimer > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, out hit, distWall))
            {
                if (hit.collider == wallCollider)
                {
                    PlayerAnimator.SetBool("ClimbWall", true);
                    isClimbing = true;
                    ResetImpactY();
                    //Gives the Character movement on the Y axis to climb up the wall
                    CharController.Move(new Vector3(0f, climbSpeed * Time.deltaTime, 0f));
                    yield return null;
                    if (inHand == true)
                    {
                        sphere.SetActive(false);
                        inHandStored = inHand;
                        inHand = false;
                    }
                    else
                    {
                        PlayerAnimator.SetBool("ClimbWall", false);
                        isClimbing = false;
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

            ResetImpactY();
            AddForce(Vector3.up, edgeUpForce);
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

            if (Input.GetKey(KeyCode.E) || Input.GetButton(ControllerInteractString))
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
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        shootingScript.DodgeballsInHand = 0;
        yield return new WaitForSeconds(3);
        transform.position = SpawnPointList[tempRandomNum].transform.position;
        transform.eulerAngles = SpawnPointList[tempRandomNum].transform.eulerAngles;
        isRespawning = false;
        nextDash = nextDash - dashCooldown;
        isDashCooldown = true;
        dashCooldownIMG.fillAmount = 1f;
        RespawningText.color = new Color(RespawningText.color.r, RespawningText.color.g, RespawningText.color.b, 0f);
        HealthNumText.text = PlayerHealth.originalHealth.ToString() + "/" + PlayerHealth.originalHealth.ToString();
        filledHealthbarIMG.fillAmount = PlayerHealth.originalHealth / PlayerHealth.originalHealth;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    public IEnumerator Hitmarker()
    {
        hitmarkerIMG.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitmarkerIMG.gameObject.SetActive(false);
    }

    void vision()
    {
        if (!blockVision1)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                p1Vision.enabled = true;
                blockVision1 = true;
            }
        }

        if (!blockVision2)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                p2Vision.enabled = true;
                blockVision2 = true;
            }
        }

        if (blockVision1)
        {
            visionTime1 += Time.deltaTime;

            if (visionTime1 >= 5.0f)
            {
                p1Vision.enabled = false;
                visionTime1 = 0.0f;
            }
        }

        if (blockVision2)
        {
            visionTime2 += Time.deltaTime;

            if (visionTime2 >= 5.0f)
            {
                p2Vision.enabled = false;
                visionTime2 = 0.0f;
            }
        }
    }
}
