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

            if (capsule.transform.localScale.x > newScale)
            {
                capsuleObj.transform.localScale -= new Vector3(0.01f, 0.0f, 0.01f);
                CatchReticleImg.transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            }
        }
        else
        {
            CatchReticleImg.gameObject.SetActive(false);
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

/*public Image CatchReticleImg;
private Color scaleColour = Color.blue;
public RectTransform CatchReticleRT;
private Collider capsule;
public GameObject sphere;
public GameObject capsuleObj;
public PlayerMovementControllerNoNetwork controller;
public ShootingNoNetwork shooter;
private GameManagerScriptNoNetwork GMScript;
private float oldScale = 1.75f;
private float newScale = 0.5f;
private float scaleDifference = 0.0f;
private float scalePercentage = 0.0f;

private string ControllerCatchString;

// Start is called before the first frame update
void Start()
{
    GMScript = FindObjectOfType<GameManagerScriptNoNetwork>();
    CatchReticleRT.sizeDelta = new Vector2(410 / GMScript.NumberOfPlayers, 410 / GMScript.NumberOfPlayers);
    CatchReticleImg.gameObject.SetActive(false);
    ControllerCatchString = "ControllerCatch" + GetComponentInParent<PlayerRootInfo>().PlayerID.ToString();
    capsule = GetComponent<CapsuleCollider>();
    scaleDifference = oldScale = newScale;
}

private void FixedUpdate()
{
    if (Input.GetKey(KeyCode.Q) || Input.GetAxis(ControllerCatchString) != 0)
    {
        CatchReticleImg.gameObject.SetActive(true);

        if (capsule.transform.localScale.x > 1.75f)
        {
            capsuleObj.transform.localScale -= new Vector3(0.01f, 0.0f, 0.01f);
            CatchReticleImg.transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            scalePercentage = (oldScale - CatchReticleImg.transform.localScale.x) / scaleDifference;
            scaleColour = Color.Lerp(Color.blue, Color.red, scalePercentage);
            CatchReticleImg.color = scaleColour;
        }
    }
    else
    {
        CatchReticleImg.gameObject.SetActive(false);
        if (capsule.transform.localScale.x < 0.5f)
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
}*/