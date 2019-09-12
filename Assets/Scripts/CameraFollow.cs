using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : MonoBehaviour
{

    public Camera PlayerCam;
    private GameObject spine;
    private float z = 0;
    private float zMinLimit = -45.00f;
    private float zMaxLimit = 45.00f;
    private GameObject Player;
    public float CameraMoveSpeed = 120.0f;
    private GameObject CameraFollowObj;
    Vector3 FollowPOS;
    public float clampAngle = 60.0f;
    public float inputSens = 150.0f;
    public float inputSensPlayer = 2.0f;
    public GameObject CamerObj;
    public GameObject PlayerObj;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    public float rotateSpeed = 2;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private float baseFOV;
    float scopedFOV = 40.0f;

    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        CameraFollowObj = GameObject.FindGameObjectWithTag("CameraFollow");
        baseFOV = PlayerCam.fieldOfView;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * inputSens * Time.deltaTime;
        rotX += finalInputZ * inputSens * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        Quaternion localPlayerRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = localRotation;
        Player.transform.localRotation = localPlayerRotation;
	}

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            PlayerCam.fieldOfView = scopedFOV;
        }
        else
        {
            PlayerCam.fieldOfView = baseFOV;
        }
    }

    void LateUpdate()
    {
        z += Input.GetAxis("Mouse Y") * rotateSpeed;
        z = ClampAngle(z, zMinLimit, zMaxLimit);
        CameraUpdater();
    }
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180)
        {
            angle += 180;
        }
        if (angle > 180)
        {
            angle -= 180;
        }
        return Mathf.Clamp(angle, min, max);
    }

    void CameraUpdater()
    {
        Transform target = CameraFollowObj.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
