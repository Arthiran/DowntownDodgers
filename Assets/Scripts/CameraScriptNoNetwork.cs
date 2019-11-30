using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CameraScriptNoNetwork : MonoBehaviour
{
    private float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;

    //Networking
    private PhotonView PV;
    //Get Character properties
    public GameObject Player;
    public GameObject PlayerEmptyChild;
    public PlayerMovementControllerNoNetwork MovementController;

    //Initialize Variables
    public float mouseX;
    public float mouseY;
    public float RightAnalogX;
    public float RightAnalogY;
    public float inputSens = 150.0f;
    [Range(0.0f, 1.0f)]
    public float aimAssistFactor = 0.5f;
    public float controllerInputSens = 150.0f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    private float clampAngle = 90.0f;
    private float distWall = 1f;
    private string RightAnalogXString;
    private string RightAnalogYString;
    private bool hitOuterCollision;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            RightAnalogXString = "RightAnalogX" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
            RightAnalogYString = "RightAnalogY" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
        }
        else
        {
            RightAnalogXString = "RightAnalogX1";
            RightAnalogYString = "RightAnalogY1";
        }
        //Finds the character which has a tag set to Player
        //Rotation variables are set
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        //Locks the mouse to the center of the screen and then hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame, Camera movement should always be in LateUpdate()
    private void Update()
    {
        //Gets mouse input and stores them in a variable
        if (Cursor.visible == false)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            RightAnalogX = Input.GetAxis(RightAnalogXString);
            RightAnalogY = Input.GetAxis(RightAnalogYString);
        }
        else if (Cursor.visible == true)
        {
            mouseX = 0f;
            mouseY = 0f;
            RightAnalogX = 0f;
            RightAnalogY = 0f;
        }

        //Takes the mouse input and multiplies it by the sensitivity and then multiplied by Time.deltaTime for frame rate
        rotY += mouseX * inputSens * Time.deltaTime;
        rotX -= mouseY * inputSens * Time.deltaTime;

        rotY += RightAnalogX * inputSens * Time.deltaTime;
        rotX -= RightAnalogY * inputSens * Time.deltaTime;

        //Clamps the rotation vertically so you can't view things upside down
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //Assigns the rotation to a variable and also sets a variable for the rotation of the player
        //Actually changes the rotation
        transform.eulerAngles = new Vector3(rotX, rotY, 0.0f);

        PlayerEmptyChild.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
        RaycastHit hit;
        if (Physics.Raycast(Player.transform.position, PlayerEmptyChild.transform.transform.transform.forward, out hit, distWall) == false)
        {
            Player.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
        }
        else
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag != "Climable")
                {
                    Player.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
                }
                else if (hit.collider.tag == "Climable" && MovementController.isClimbing == false)
                {
                    Player.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
                }
            }
        }

        //This is so that the Camera can follow the Player
        transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y + 0.61f, Player.transform.localPosition.z);


        if (MovementController.isGrounded)
        {
            float waveslice = 0.0f;

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingAmount;
                float totalAxes = Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Input.GetAxisRaw("Vertical");
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + translateChange, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }
}
