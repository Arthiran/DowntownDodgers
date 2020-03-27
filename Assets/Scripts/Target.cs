using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class Target : MonoBehaviour {

    //Initialize Variables
    public float health = 30f;
    public float originalHealth;
    public PlayerMovementController MovementController;
    private GameManagerScript gameManager;
    public Image vignette;
    bool toFade = false;
    bool vibrate;
    int controllerID;
    float vibeTimer = 0.0f;

    XInputTestCS controller;

    private void Start()
    {
        originalHealth = health;
        MovementController = GetComponent<PlayerMovementController>();
        controller = GameObject.FindObjectOfType<XInputTestCS>();
        vignette = vignette.GetComponent<Image>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

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
            GamePad.SetVibration((PlayerIndex)controllerID, 1, 1);
            //Debug.Log("vibe");
            vibeTimer += Time.deltaTime;
            if (vibeTimer >= 0.2f)
            {
                GamePad.SetVibration((PlayerIndex)controllerID, 0, 0);
                vibrate = false;
                vibeTimer = 0.0f;
            }
            //Debug.Log(vibrate);
        }
    }

    private void RumbleOnHit(int controllerID)
    {
        GamePad.SetVibration((PlayerIndex)controllerID, 1, 1);
        //Debug.Log("vibe");
        vibeTimer += Time.deltaTime;
        if (vibeTimer >= 0.2f)
        {
            GamePad.SetVibration((PlayerIndex)controllerID, 0, 0);
            //Debug.Log("no vibe");
            vibrate = false;
            vibeTimer = 0.0f;
        }
        Debug.Log(vibrate);
    }

    //Calculates the amount of damage taken from shot, dies if under 0 health
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (gameObject.tag == "Player")
        {
            //Show vignette
            if (MovementController.PlayerID == 1)
            {
                vibrate = true;
                //if (vibrate)
                //RumbleOnHit(0);
                MovementController.PlayerAnimator.SetBool("Stunned", true);
                StartCoroutine(MovementController.HitStun());
                controllerID = 0;
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
                //Debug.Log("Vignetter1");
            }
            if (MovementController.PlayerID == 2)
            {
                vibrate = true;
                //if (vibrate)
                //RumbleOnHit(1);
                StartCoroutine(MovementController.HitStun());
                controllerID = 1;
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
                //Debug.Log("Vignetter2");
            }
            if (MovementController.PlayerID == 3)
            {
                vibrate = true;
                //if (vibrate)
                //RumbleOnHit(1);
                StartCoroutine(MovementController.HitStun());
                controllerID = 2;
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
                //Debug.Log("Vignetter2");
            }
            if (MovementController.PlayerID == 4)
            {
                vibrate = true;
                //if (vibrate)
                //RumbleOnHit(1);
                StartCoroutine(MovementController.HitStun());
                controllerID = 3;
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
                //Debug.Log("Vignetter2");
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
                    //Debug.Log("Vignetterrrr1");
                }
                if (MovementController.PlayerID == 2)
                {
                    //vignette.CrossFadeAlpha(1, 2.0f, false);
                    var temp = vignette.color;
                    vignette.CrossFadeAlpha(0, 1.5f, false);
                    temp.a = 1;
                    vignette.color = temp;
                    toFade = false;
                    //Debug.Log("Vignetterrrr2");
                }
                if (MovementController.PlayerID == 3)
                {
                    //vignette.CrossFadeAlpha(1, 2.0f, false);
                    var temp = vignette.color;
                    vignette.CrossFadeAlpha(0, 1.5f, false);
                    temp.a = 1;
                    vignette.color = temp;
                    toFade = false;
                    //Debug.Log("Vignetterrrr2");
                }
                if (MovementController.PlayerID == 4)
                {
                    //vignette.CrossFadeAlpha(1, 2.0f, false);
                    var temp = vignette.color;
                    vignette.CrossFadeAlpha(0, 1.5f, false);
                    temp.a = 1;
                    vignette.color = temp;
                    toFade = false;
                    //Debug.Log("Vignetterrrr2");
                }
            }
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DodgeballScript dodgeballScript = collision.gameObject.GetComponent<DodgeballScript>();
        if (dodgeballScript != null)
        {
            if (dodgeballScript.inAir == true)
            {
                TakeDamage(dodgeballScript.damage);
                if (dodgeballScript.MovementController != null)
                {
                    dodgeballScript.MovementController.StartCoroutine(dodgeballScript.MovementController.Hitmarker());
                }
                Destroy(collision.gameObject);

            }
        }
    }

    //Destroys game object
    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            if (MovementController != null)
            {
                //Destroy(MovementController.GetComponentInParent<PlayerRootInfo>().gameObject);
                MovementController.StartCoroutine(MovementController.Respawn());
                //health = originalHealth;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
