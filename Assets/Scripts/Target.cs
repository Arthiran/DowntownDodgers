using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using XInputDotNetPure;

public class Target : MonoBehaviour {

    //Initialize Variables
    public float health = 30f;
    public float originalHealth;
    private PlayerMovementControllerNoNetwork MovementController;
    public GameManagerScriptNoNetwork gameManager;
    public Image vignette;
    bool toFade = false;
    private PhotonView PV;
    bool vibrate;
    float vibeTimer = 0.0f;

    XInputTestCS controller;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        /*if (PV.IsMine)
        {
            originalHealth = health;
            MovementController = GetComponent<PlayerMovementController>();
            if (gameObject.tag == "Player")
            {
                MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
                MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
            }
        }*/
        originalHealth = health;
        MovementController = GetComponent<PlayerMovementControllerNoNetwork>();
        controller = GameObject.FindObjectOfType<XInputTestCS>();
        vignette = vignette.GetComponent<Image>();

        if (gameObject.tag == "Player")
        {
            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
        }
    }

    private void FixedUpdate()
    {
        if (vibrate == true)
        {
            GamePad.SetVibration(0, 1, 1);
            Debug.Log("vibe");
            vibeTimer += Time.deltaTime;
            if (vibeTimer >= 0.2f)
            {
                GamePad.SetVibration(0, 0, 0);
                Debug.Log("no vibe");
                vibrate = false;
                vibeTimer = 0.0f;
            }
            Debug.Log(vibrate);
        }
    }

    private void RumbleOnHit(PlayerIndex controllerID)
    {
        GamePad.SetVibration(controllerID, 1, 1);
        Debug.Log("vibe");
        vibeTimer += Time.deltaTime;
        if (vibeTimer >= 0.2f)
        {
            GamePad.SetVibration(controllerID, 0, 0);
            Debug.Log("no vibe");
            vibrate = false;
            vibeTimer = 0.0f;
        }
        Debug.Log(vibrate);
    }

    //Calculates the amount of damage taken from shot, dies if under 0 health
    //[PunRPC]
    public void TakeDamage(float amount)
    {
        /*if (PV.IsMine)
        {
            health -= amount;
            if (gameObject.tag == "Player")
            {
                MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
                MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
            }
            if (health <= 0f)
            {
                Die();
            }
        }*/
        health -= amount;
        if (gameObject.tag == "Player")
        {
            //Show vignette
            if (MovementController.PlayerID == 1)
            {
                vibrate = true;
                if (vibrate)
                    RumbleOnHit((PlayerIndex)1);
                vignette.canvasRenderer.SetAlpha(1.0f);
                vignette.enabled = true;
                var temp = vignette.color;
                temp.a = 1;
                vignette.color = temp;
                //vignette.CrossFadeAlpha(0, 1.5f, false);
                //vignette.enabled = false;
                //temp.a = 1;
                //vignette.color = temp;
                toFade = true;
                Debug.Log("Vignetter1");
            }
            if (MovementController.PlayerID == 2)
            {
                vibrate = true;
                if (vibrate)
                    RumbleOnHit((PlayerIndex)0);
                vignette.canvasRenderer.SetAlpha(1.0f);
                vignette.enabled = true;
                var temp = vignette.color;
                temp.a = 1;
                vignette.color = temp;
                //vignette.CrossFadeAlpha(0, 1.5f, false);
                //vignette.enabled = false;
                //temp.a = 1;
                //vignette.color = temp;
                toFade = true;
                Debug.Log("Vignetter2");
            }

            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;

            if (toFade)
            {
                //Fade vignette
                if (MovementController.PlayerID == 1)
                {
                    //vignette.CrossFadeAlpha(1, 2.0f, false);
                    var temp = vignette.color;
                    vignette.CrossFadeAlpha(0, 1.5f, false);
                    temp.a = 1;
                    vignette.color = temp;
                    toFade = false;
                    Debug.Log("Vignetterrrr1");
                }
                if (MovementController.PlayerID == 2)
                {
                    //vignette.CrossFadeAlpha(1, 2.0f, false);
                    var temp = vignette.color;
                    vignette.CrossFadeAlpha(0, 1.5f, false);
                    temp.a = 1;
                    vignette.color = temp;
                    toFade = false;
                    Debug.Log("Vignetterrrr2");
                }
            }
        }

        if (health <= 0f)
        {
            //Increase scores
            if (MovementController.PlayerID == 1)
            {
                gameManager.p2Score++;
                Debug.Log(gameManager.p2Score);
            }
            if (MovementController.PlayerID == 2)
            {
                gameManager.p1Score++;
                Debug.Log(gameManager.p1Score);
            }

            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (PV.IsMine)
        {
            DodgeballScript dodgeballScript = collision.gameObject.GetComponent<DodgeballScript>();
            if (dodgeballScript != null)
            {
                TakeDamage(dodgeballScript.damage);
                Destroy(collision.gameObject);
            }
        }*/
        DodgeballScript dodgeballScript = collision.gameObject.GetComponent<DodgeballScript>();
        if (dodgeballScript != null)
        {
            TakeDamage(dodgeballScript.damage);
            Destroy(collision.gameObject);
        }
    }

    //Destroys game object
    void Die()
    {
        /*if (PV.IsMine)
        {
            if (gameObject.tag == "Player")
            {
                if (MovementController != null)
                {
                    MovementController.StartCoroutine(MovementController.Respawn());
                    health = originalHealth;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }*/

        if (gameObject.tag == "Player")
        {
            if (MovementController != null)
            {
                MovementController.StartCoroutine(MovementController.Respawn());
                health = originalHealth;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
