using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public float damage = 10.0f;
    public float range = 1000.0f;
    public float fireRate = 10000.0f;

    private Camera PlayerCam;

    private float nextTimeToFire = 0f;

    public Transform BallCarrierTransform;
    public GameObject DodgeballPrefab;
    public float delayBetweenDodgeballs;
    public float dodgeballLaunchForce;
    private List<Transform> projectileSpawnTransforms = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        PlayerCam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LeftClickAttack());
        }

        /*if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(DodgeballPrefab, BallCarrierTransform.transform.position, Quaternion.identity);
        }*/
    }

    private IEnumerator LeftClickAttack()
    {
        int DodgeballsFired = 0;

        while (DodgeballsFired < 1)
        {
            DodgeballsFired++;
            GameObject DodgeballInstance = Instantiate(DodgeballPrefab, BallCarrierTransform.position, BallCarrierTransform.rotation);
            DodgeballInstance.GetComponent<Rigidbody>().AddForce(DodgeballInstance.transform.forward * dodgeballLaunchForce, ForceMode.VelocityChange);
            yield return new WaitForSeconds(delayBetweenDodgeballs);
        }

        yield return null;
    }

    /*void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }*/

}
