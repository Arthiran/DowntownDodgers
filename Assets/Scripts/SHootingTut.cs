using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingTut : MonoBehaviour
{
    //Get transform to spawn dodgeball in and get dodgeball prefab
    public Transform BallShootingTransform;
    public GameObject DodgeballPrefab;
    public Image[] FillSlots;
    public Text dodgeballsText;
    public Text inHandText;
    private PlayerMovementController MovementController;

    //Initializes Variables
    public float dodgeballLaunchForce;
    public float fireRate;
    private float nextFire;
    public int DodgeballsInHand = 0;
    public int DodgeballCarryLimit = 3;

    private string ControllerShootString;

    public GiveQuest giveQuest;

    private void Start()
    {
        MovementController = GetComponentInChildren<PlayerMovementController>();
        ControllerShootString = "ControllerShoot1"; //+ GetComponent<PlayerRootInfo>().PlayerID.ToString();
    }

    //Checks if left mouse button was clicked
    void Update()
    {
        if ((Input.GetMouseButton(0) || (Input.GetAxisRaw(ControllerShootString) > 0)) && DodgeballsInHand > 0 && DodgeballsInHand <= 3 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //Starts dodgeball shooting coroutine
            Shoot();
        }

        for (int i = 0; i < DodgeballsInHand; i++)
        {
            if (DodgeballsInHand != 0)
            {
                FillSlots[i].fillAmount = 1f;
            }
        }

        if (DodgeballsInHand == 0)
        {
            for (int i = 0; i < FillSlots.Length; i++)
                FillSlots[i].fillAmount = 0f;
        }

        dodgeballsText.text = DodgeballsInHand.ToString() + "/" + DodgeballCarryLimit.ToString();
    }

    //Fires Dodgeball
    private void Shoot()
    {
        giveQuest.loadQuest(2);
        //Spawns an instance of the dodgeball prefab at the spawn transform
        GameObject DodgeballInstance = Instantiate(DodgeballPrefab, BallShootingTransform.position, BallShootingTransform.rotation);
        //Gives dodgeball a launch force wherever the character is facing
        DodgeballInstance.GetComponent<Rigidbody>().AddForce(DodgeballInstance.transform.forward * dodgeballLaunchForce, ForceMode.VelocityChange);
        FillSlots[DodgeballsInHand - 1].fillAmount = 0f;
        DodgeballsInHand--;
    }
}
