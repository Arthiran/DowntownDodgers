using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catch : MonoBehaviour
{
    public Image CatchReticleImg;
    private Collider capsule;
    public GameObject sphere;
    public GameObject capsuleObj;
    public PlayerMovementControllerNoNetwork controller;
    public ShootingNoNetwork shooter;

    private string ControllerCatchString;

    // Start is called before the first frame update
    void Start()
    {
        CatchReticleImg.gameObject.SetActive(false);
        ControllerCatchString = "ControllerCatch" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
        capsule = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetAxis(ControllerCatchString) != 0)
        {
            CatchReticleImg.gameObject.SetActive(true);

            if (capsule.transform.localScale.x > 0.5f)
            {
                capsuleObj.transform.localScale -= new Vector3(0.01f, 0.0f, 0.01f);
                CatchReticleImg.transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            }
        }
        else
        {
            CatchReticleImg.gameObject.SetActive(false);
            if (capsule.transform.localScale.x < 1.75f)
            {
                capsuleObj.transform.localScale += new Vector3(0.005f, 0.0f, 0.005f);
                CatchReticleImg.transform.localScale += new Vector3(0.005f, 0.005f, 0.0f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetAxis(ControllerCatchString) != 0)
        {
            if (other.gameObject.GetComponent<DodgeballScript>() != null)
            {
                if (!controller.inHand)
                {
                    if (!sphere.activeSelf)
                    {
                        if (other.gameObject.GetComponent<DodgeballScript>().inAir)
                        {
                            Destroy(other.gameObject);
                            shooter.DodgeballsInHand++;
                        }
                    }
                }
            }
        }
    }
}
