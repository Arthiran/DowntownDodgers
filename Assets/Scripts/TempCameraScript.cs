using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraScript : MonoBehaviour
{
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float inputSens = 150.0f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    public float clampAngle = 60.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private GameObject Player;
    float xClamp;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * inputSens * Time.deltaTime;
        rotX -= finalInputZ * inputSens * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        Quaternion localPlayerRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = localRotation;
        Player.transform.rotation = localPlayerRotation;

        transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y + 0.6f, Player.transform.localPosition.z);
    }
}
