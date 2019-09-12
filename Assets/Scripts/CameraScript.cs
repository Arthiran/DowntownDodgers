using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Get Character properties
    private GameObject Player;

    //Initialize Variables
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float inputSens = 150.0f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    public float clampAngle = 60.0f;

    void Start()
    {
        //Finds the character which has a tag set to Player
        Player = GameObject.FindGameObjectWithTag("Player");
        //Rotation variables are set
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        //Locks the mouse to the center of the screen and then hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame, Camera movement should always be in LateUpdate()
    void LateUpdate()
    {
        //Gets mouse input and stores them in a variable
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //Takes the mouse input and multiplies it by the sensitivity and then multiplied by Time.deltaTime for frame rate
        rotY += mouseX * inputSens * Time.deltaTime;
        rotX -= mouseY * inputSens * Time.deltaTime;

        //Clamps the rotation vertically so you can't view things upside down
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //Assigns the rotation to a variable and also sets a variable for the rotation of the player
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        Quaternion localPlayerRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        //Actually changes the rotation
        transform.rotation = localRotation;
        Player.transform.rotation = localPlayerRotation;

        //This is so that the Camera can follow the Player
        transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y + 0.6f, Player.transform.localPosition.z);
    }
}