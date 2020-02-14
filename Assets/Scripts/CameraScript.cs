using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    private float timer = 0.0f;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;

    //Get Character properties
    public GameObject Player;
    public GameObject PlayerEmptyChild;
    public PlayerMovementController MovementController;

    //Initialize Variables
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 FollowPOS;
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
    private float clampAngle = 35.0f;
    private float distWall = 1f;
    private string RightAnalogXString;
    private string RightAnalogYString;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                RightAnalogXString = "RightAnalogX1";
                RightAnalogYString = "RightAnalogY1";
            }
            else
            {
                RightAnalogXString = "RightAnalogX" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
                RightAnalogYString = "RightAnalogY" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
            }


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

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

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

		if (MovementController.isGrounded)
		{
			float waveslice = 0.0f;

			//Debug.Log(MovementController.horizontalMovement);
			if ((MovementController.horizontalMovement == 0 && MovementController.forwardMovement == 0))
			{
				timer = 0.0f;
				//Debug.Log("HALLO");
			}
			else
			{
				waveslice = Mathf.Sin(timer);
				timer = timer + bobbingSpeed * Time.deltaTime;
				if (timer > Mathf.PI * 2)
				{
					timer = timer - (Mathf.PI * 2);
				}
			}
			if (waveslice != 0)
			{
				float translateChange = waveslice * bobbingAmount;
				float totalAxes = Mathf.Abs(MovementController.horizontalMovement + MovementController.forwardMovement);
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
    private void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        Transform target = CameraFollowObj.transform;
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
