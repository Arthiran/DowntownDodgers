using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    public GiveQuest quest;
    //Get Character Properties
    private Transform Player;
    [HideInInspector]
    public Animator PlayerAnimator;
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
    private Shooting shootingScript;
    private GameObject DodgeballInstance;
    private Target PlayerHealth;
    private GameObject[] SpawnPointList;
    private FMODUnity.StudioEventEmitter[] eventEmitter;
  


    //public GameObject DodgeballPrefab;
    public Transform BallCarrierTransform;

    public GameObject sphere;

    //Player materials
    public Material player1Mat;
    public Material player2Mat;
    public Material respawn1Mat;
    public Material respawn2Mat;
    public Material despawn1Mat;
    public Material despawn2Mat;

    private float despawnVal = 0;
    private float spawnVal = 0;
    private bool isDissolved = false;
    private bool isSpawning = false;
    private bool isPlaying = false;
    [HideInInspector]
    public bool stunned = false;
    
    private bool isDashCooldown;

    //Initialize Variables
    private int soundCounter = 0;
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
    private float edgeUpForce = 50f;
    public float climbSpeed = 0f;
    public float jumpForce = 4f;
    public float dashForce = 4f;
    public float dashCooldown = 5f;
    public float hitStunDuration = 1.0f;
    private float dashCountdownUI;
    private float nextDash;
    private float gravity = Physics.gravity.y;
    private float distWall = 0.7f;
    public bool isGrounded = false;
    public bool isClimbing = false;
    private bool isFalling = false;
    private bool isRespawning = false;
    public float climbWallTimer = 1f;
    private float tempClimbTimer;
    private float respawnTimer = 1f;
    private float tempRespawnTimer;
    private int movementSoundCounter = 0;
    public int tempRandomNum;
    public int PlayerID;

    public float forwardMovement;
    public float horizontalMovement;

    private int doubleJumpCheck = 0;
    private int climbWallCheck = 0;

    //ControllerStrings
    private string LeftAnalogXString;
    private string LeftAnalogYString;
    private string ControllerJumpString;
    private string ControllerDashString;
    private string ControllerBackwardDashString;
    private string ControllerLoadString;
    private string ControllerInteractString;

    private void Start()
    {
        PlayerID = GetComponentInParent<PlayerRootInfo>().PlayerID;

        if (PlayerID == 1)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material = player1Mat;
            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        }
        else if (PlayerID == 2)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material = player2Mat;
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;
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
        shootingScript = GetComponentInParent<Shooting>();
        PlayerHealth = GetComponent<Target>();
        eventEmitter = GetComponentsInParent<FMODUnity.StudioEventEmitter>();

        tempClimbTimer = climbWallTimer;
        tempRespawnTimer = respawnTimer;

        dashCountdownUI = dashCooldown + 1;

        //sphere.SetActive(true);

        shootingScript.DodgeballsInHand = 1;
    }

    //Any Input(Keyboard or Mouse) should be in Update function
    private void Update()
    {
        if (isSpawning)
        {
            spawnVal += Time.deltaTime;
            //Respawn Effect
            if (PlayerID == 1)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = respawn1Mat;
                respawn1Mat.SetFloat("val", spawnVal);
                //GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
            }
            else if (PlayerID == 2)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = respawn2Mat;
                respawn2Mat.SetFloat("val", spawnVal);
                //GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
        }
        //Checks if Player is on the ground, if true set Y Velocity to 0
        if (isGrounded && velocityY < 0f)
        {
            doubleJumpCheck = 0;
            climbWallCheck = 0;
            isClimbing = false;
            PlayerAnimator.SetBool("ClimbWall", false);
            PlayerAnimator.SetBool("Jump", false);
            isFalling = false;
            tempClimbTimer = climbWallTimer;
            tempRespawnTimer = respawnTimer;
            velocityY = 0f;   
        }

        



        //Get Input from Horizontal and Vertical Axis and store them in variables
        if ((Input.GetAxisRaw("Vertical") != 0) && stunned == false)
        {
            vertical = Input.GetAxis("Vertical");
            forwardMovement = vertical;
            movementSoundCounter++;

        }
        else if ((Input.GetAxis(LeftAnalogYString) != 0) && stunned == false)
        {
            LeftAnalogY = Input.GetAxis(LeftAnalogYString);
            forwardMovement = LeftAnalogY;
            movementSoundCounter++;

        }
        else
        {
            if (stunned == false)
            {
                forwardMovement = Input.GetAxis("Vertical");
            }
        }

        if (isClimbing == false)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0) && stunned == false)
            {
                horizontal = Input.GetAxis("Horizontal");
                horizontalMovement = horizontal;
                movementSoundCounter++;


            }
            else if ((Input.GetAxis(LeftAnalogXString) != 0) && stunned == false)
            {
                LeftAnalogX = Input.GetAxis(LeftAnalogXString);
                horizontalMovement = LeftAnalogX;
                movementSoundCounter++;

                //	Debug.Log("Hello");
            }
            else
            {
                if (stunned == false)
                {
                    horizontalMovement = Input.GetAxis("Horizontal");
                }
            } 
            Dash();
            //BackwardDash();
        }

        /*This is so that when you press W and A at the same time for instance, the player doesn't become faster,
        it remains the same speed*/
        if (forwardMovement != 0 && horizontalMovement != 0)
        {
            forwardMovement *= 0.7071f;
            horizontalMovement *= 0.7071f;
            //Debug.Log("Bruv");
        }

        //Create a Vector to store the overall movement
        movement = new Vector3(horizontalMovement, 0, forwardMovement);
        Vector3 velocity;

        /*Takes the movement vector and converts the position from Local Space to World Space and stores 
        it back in the movement variable*/
        movement = transform.TransformDirection(movement);

        //Calculates gravity and stores it in variable
        velocityY += gravity * Time.deltaTime;

        //Vector which stores the overall effect of gravity on the Character's position
        velocity = movement * moveSpeed * Time.deltaTime * 1.2f + Vector3.up * velocityY;

        //sets the velocity to take all forces into account
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }
        //Takes the velocity and actually moves the Character
        if (isRespawning == false && gameObject.GetComponent<CharacterController>().enabled)
        {
            CharController.Move(velocity * Time.fixedDeltaTime);
            //     FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep", GetComponent<Transform>().position);

        }
        isGrounded = CharController.isGrounded;
        //Animations 
        PlayerAnimator.SetFloat("Vertical", forwardMovement);
        PlayerAnimator.SetFloat("Horizontal", horizontalMovement);

        //Jump pls
        Jump();
        //Interpolates the effects of forces for smooth movement
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);

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

        if (isRespawning)
        {
            //Debug.Log("Dissolve");
            if (!isDissolved)
            {
                despawnVal += Time.deltaTime;
                if (PlayerID == 1)
                {
                    GetComponentInChildren<SkinnedMeshRenderer>().material = despawn1Mat;
                    despawn1Mat.SetFloat("val", despawnVal);
                    //GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                }
                else if (PlayerID == 2)
                {
                    GetComponentInChildren<SkinnedMeshRenderer>().material = despawn2Mat;
                    despawn2Mat.SetFloat("val", despawnVal);
                    //GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                }
            }

            if (isGrounded)
            {
                PlayerAnimator.SetBool("Jump", false);
                if ((horizontalMovement != 0 || forwardMovement != 0) && !eventEmitter[0].IsPlaying())
                {
                    eventEmitter[0].Play();
                }
            }
        }
    }

    //All Physics and Movement should be handled in FixedUpdate()
    private void FixedUpdate()
    {

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
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown(ControllerJumpString)) && stunned == false)
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

        if ((Input.GetButton("Jump") || Input.GetButton(ControllerJumpString)) && stunned == false && climbWallCheck == 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, out hit, distWall))
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Dash", GetComponent<Transform>().position);
                if (hit.collider.tag == "Climable" && (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw(LeftAnalogYString) > 0))
                {
                    StartCoroutine(ClimbWall(hit.collider));
                    climbWallCheck++;
                }
            }
        }
        else if (Input.GetButtonUp("Jump") || Input.GetButtonUp(ControllerJumpString))
        {
            climbWallCheck = 0;
        }
    }

    private void Dash()
    {
        if (((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetAxisRaw(ControllerDashString) > 0) && Time.time > nextDash) && stunned == false)
        {
            StartCoroutine(DashAnimation());
            isDashCooldown = true;
            nextDash = Time.time + dashCooldown;
            AddForce(transform.forward, dashForce);

            if (!eventEmitter[1].IsPlaying())
            {
                eventEmitter[1].Play();
            }
        //    Debug.Log("Hello");
        }
    }
    private void BackwardDash()
    {
        if (((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetAxisRaw(ControllerBackwardDashString) > 0) && Time.time > nextDash) && stunned == false)
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

    //Custom AddForce function, not to be mistakened with Rigidbody.AddForce()
    public void AddForce(Vector3 direction, float magnitude)
    {
        //Adds the force to the current amount of forces being applied to the game object
        currentImpact += direction.normalized * magnitude / mass;
    }

    public IEnumerator HitStun()
    {
        stunned = true;
        yield return new WaitForSeconds(hitStunDuration);
        stunned = false;
        PlayerAnimator.SetBool("Stunned", false);
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
                    PlayerAnimator.SetBool("Jump", false);
                    isClimbing = true;
                    ResetImpactY();
                    //Gives the Character movement on the Y axis to climb up the wall
                    CharController.Move(new Vector3(0f, climbSpeed * Time.deltaTime, 0f));
                    yield return null;
                    PlayerAnimator.SetBool("ClimbWall", false);
                    isClimbing = false;
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
        gameObject.GetComponent<CharacterController>().enabled = false;
        shootingScript.DodgeballsInHand = 0;
        yield return new WaitForSeconds(1);
        despawnVal = 0.0f;
        spawnVal = 0.0f;
        isDissolved = true;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        //yield return new WaitForSeconds(2);
        transform.position = SpawnPointList[tempRandomNum].transform.position;
        transform.eulerAngles = SpawnPointList[tempRandomNum].transform.eulerAngles;
        isRespawning = false;
        isDissolved = false;
        nextDash = nextDash - dashCooldown;
        isDashCooldown = true;
        dashCooldownIMG.fillAmount = 1f;
        RespawningText.color = new Color(RespawningText.color.r, RespawningText.color.g, RespawningText.color.b, 0f);
        HealthNumText.text = PlayerHealth.originalHealth.ToString() + "/" + PlayerHealth.originalHealth.ToString();
        filledHealthbarIMG.fillAmount = PlayerHealth.originalHealth / PlayerHealth.originalHealth;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        yield return new WaitForSeconds(1);
        isSpawning = true;
        yield return new WaitForSeconds(1);
        isSpawning = false;
        gameObject.GetComponent<CharacterController>().enabled = true;
        stunned = false;
    }

    public IEnumerator Hitmarker()
    {
        hitmarkerIMG.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitmarkerIMG.gameObject.SetActive(false);
    }
}
