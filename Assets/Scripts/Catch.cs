using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{
    private Collider capsule;
    public GameObject sphere;
    public GameObject capsuleObj;
    public PlayerMovementControllerNoNetwork controller;
    public ShootingNoNetwork shooter;
    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        //capsule.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //capsule.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //capsule.GetComponent<CapsuleCollider>().radius -= 0.02f;
            //capsuleObj.transform.localScale = new Vector3(capsuleObj.transform.localScale.x - 0.02f, 0.0f, capsuleObj.transform.localScale.z - 0.02f);
            if (capsule.transform.localScale.x > 0.7f)
                capsuleObj.transform.localScale -= new Vector3(0.005f, 0.0f, 0.005f);
            //capsule.transform.localScale -= new Vector3(0.02f, 0.0f, 0.02f);
        }
        else
        {
            if (capsule.transform.localScale.x < 2.0f)
                capsuleObj.transform.localScale += new Vector3(0.005f, 0.0f, 0.005f);
            //capsule.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Hey bb");
            //capsule.enabled = true;

            if (other.gameObject.GetComponent<DodgeballScript>() != null)
            {
                if (!controller.inHand)
                {
                    if (!sphere.activeSelf)
                    {
                        Destroy(other.gameObject);
                        shooter.DodgeballsInHand++;
                        //sphere.SetActive(true);
                    }
                }
            }
        }
    }
}
