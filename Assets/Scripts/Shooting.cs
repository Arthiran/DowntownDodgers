using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviour
{
    private Animator PlayerAnimator;
    //Get transform to spawn dodgeball in and get dodgeball prefab
    public GameObject DodgeballPrefab;
    public GameObject[] BallSlots;
    public Text dodgeballsText;
    private PlayerMovementController MovementController;
    private ParticleSystem shootDust;
    public Camera camObj;
    public Transform shootTransform;
    private Transform camShootTransform;
    private Vector3 VectorDifference;
    private float distance;
    private Vector3 direction;
    private Ray camRay;
    Vector3 tempVec = new Vector3(0, 0, 0);

    public GameManagerScript gameManager;

    private Vector3 targetpoint;
    private int targetRange = 99999999;

    //Initializes Variables
    public float dodgeballLaunchForce;
    public float fireRate;
    private float nextFire;
    public int DodgeballsInHand = 0;
    public int DodgeballCarryLimit = 3;

    private string ControllerShootString;

    public GiveQuest giveQuest;
    Scene currentScene;
    string sceneName;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        shootDust = GetComponentInChildren<ParticleSystem>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        sceneName = currentScene.name;
        MovementController = GetComponentInChildren<PlayerMovementController>();
        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            ControllerShootString = "ControllerShoot" + GetComponent<PlayerRootInfo>().PlayerID.ToString();
        }
        else
        {
            ControllerShootString = "ControllerShoot1";
        }
    }

    //Checks if left mouse button was clicked
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, out hit))
        {
            camShootTransform = hit.transform;
        }
        if (((Input.GetMouseButton(0) || (Input.GetAxisRaw(ControllerShootString) > 0)) && DodgeballsInHand > 0 && DodgeballsInHand <= 3 && Time.time > nextFire) && MovementController.stunned == false && !gameManager.isCountingDown)
        {
            nextFire = Time.time + fireRate;
            //Starts dodgeball shooting coroutine
            Shoot();
        }

        Ray camRay2 = camObj.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        targetpoint = camRay2.GetPoint(targetRange);

        if (Physics.Raycast(camRay2.origin, camRay2.direction, out hit, targetRange))
        {
            tempVec = (hit.point - shootTransform.position).normalized;
        }
        else
        {
            tempVec = targetpoint;
        }

        Debug.DrawRay(shootTransform.position, tempVec * targetRange, Color.yellow);

        for (int i = 0; i < DodgeballsInHand; i++)
        {
            if (DodgeballsInHand != 0)
            {
                BallSlots[i].SetActive(true);
            }
        }

        if (DodgeballsInHand == 0)
        {
            for (int i = 0; i < BallSlots.Length; i++)
            {
                BallSlots[i].SetActive(false);
            }
        }
    }

    //Fires Dodgeball
    private void Shoot()
    {
        if (sceneName == "Tutorial")
        giveQuest.loadQuest(2);
        camRay = camObj.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits;
        hits = Physics.RaycastAll(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, 99999.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit rayHit = hits[i];
            if (rayHit.transform.gameObject.name == "Player")
            {
                direction = (rayHit.point - shootTransform.position).normalized;
            }
            else
            {
                direction = camRay.direction;
            }
        }
        /*RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), transform.forward, out hit))
        {
            direction = (hit.point - shootTransform.position).normalized;
        }
        else
        {
            direction = camRay.direction;
        }*/
        Quaternion rot = Quaternion.FromToRotation(DodgeballPrefab.transform.forward, direction);
        //Spawns an instance of the dodgeball prefab at the spawn transform
        GameObject DodgeballInstance = Instantiate(DodgeballPrefab, shootTransform.position, rot);
        DodgeballInstance.GetComponent<DodgeballScript>().parentGameObject = GetComponentInParent<PlayerRootInfo>().gameObject;
        DodgeballInstance.GetComponent<DodgeballScript>().PlayerID = GetComponent<PlayerRootInfo>().PlayerID;
        DodgeballInstance.GetComponent<DodgeballScript>().MovementController = MovementController;
        StartCoroutine(ShootAnimation());
        //Gives dodgeball a launch force wherever the character is facing
        //DodgeballInstance.GetComponent<Rigidbody>().velocity = camObj.transform.forward * dodgeballLaunchForce;
        DodgeballInstance.GetComponent<Rigidbody>().AddForce(camObj.transform.forward * dodgeballLaunchForce, ForceMode.Impulse);
        shootDust.Play();
        BallSlots[DodgeballsInHand - 1].SetActive(false);
        DodgeballsInHand--;
    }

    private IEnumerator ShootAnimation()
    {
        PlayerAnimator.SetBool("Throw", true);
        yield return new WaitForSeconds(1);
        PlayerAnimator.SetBool("Throw", false);
    }
}
