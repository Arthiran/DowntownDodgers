using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Catch : MonoBehaviour
{
    public Image CatchReticleImg;
    private Color scaleColour = Color.green;
    public RectTransform CatchReticleRT;
    public Animator PlayerAnimator;
    private Collider capsule;
    public GameObject sphere;
    public GameObject capsuleObj;
    public PlayerMovementControllerNoNetwork controller;
    public ShootingNoNetwork shooter;
    private GameManagerScriptNoNetwork GMScript;
    private float oldScale = 0.75f;
    private float newScale = 0.25f;

    private string ControllerCatchString;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "LevelEditorScene")
        {
            GMScript = FindObjectOfType<GameManagerScriptNoNetwork>();
            ControllerCatchString = "ControllerCatch" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
            CatchReticleRT.sizeDelta = new Vector2(900 / GMScript.NumberOfPlayers, 900 / GMScript.NumberOfPlayers);
        }
        else
        {
            ControllerCatchString = "ControllerCatch1";
            CatchReticleRT.sizeDelta = new Vector2(900, 900);
        }
        CatchReticleImg.gameObject.SetActive(false);

        capsule = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetAxis(ControllerCatchString) != 0)
        {
            CatchReticleImg.gameObject.SetActive(true);
            PlayerAnimator.SetBool("Catch", true);
            if (capsule.transform.localScale.x > newScale)
            {
                capsuleObj.transform.localScale -= new Vector3(0.01f, 0.0f, 0.01f);
                CatchReticleImg.transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            }
        }
        else
        {
            CatchReticleImg.gameObject.SetActive(false);
            PlayerAnimator.SetBool("Catch", false);
            if (capsule.transform.localScale.x < oldScale)
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