using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraScriptNoNetwork : MonoBehaviour
{
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
        RightAnalogXString = "RightAnalogX" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
        RightAnalogYString = "RightAnalogY" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
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
        transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y + 0.6f, Player.transform.localPosition.z);
    }
}
