using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Shooting : MonoBehaviour
{
    //Get transform to spawn dodgeball in and get dodgeball prefab
    public Transform BallCarrierTransform;
    public GameObject DodgeballPrefab;

    //Initializes Variables
    public float delayBetweenDodgeballs;
    public float dodgeballLaunchForce;

    //Checks if left mouse button was clicked
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Starts dodgeball shooting coroutine
            StartCoroutine(ShootDodgeball());
        }
    }

    //Fires Dodgeball
    private IEnumerator ShootDodgeball()
    {
        int DodgeballsFired = 0;

        while (DodgeballsFired < 1)
        {
            //Dodgeball counter
            DodgeballsFired++;
            //Spawns an instance of the dodgeball prefab at the spawn transform
            GameObject DodgeballInstance = Instantiate(DodgeballPrefab, BallCarrierTransform.position, BallCarrierTransform.rotation);
            //Gives dodgeball a launch force wherever the character is facing
            DodgeballInstance.GetComponent<Rigidbody>().AddForce(DodgeballInstance.transform.forward * dodgeballLaunchForce, ForceMode.VelocityChange);
            //Delay between shots
            yield return new WaitForSeconds(delayBetweenDodgeballs);
        }

        yield return null;
    }
}
