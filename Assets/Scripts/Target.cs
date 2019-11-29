using UnityEngine;
using Photon.Pun;

public class Target : MonoBehaviour {

    //Initialize Variables
    public float health = 30f;
    public float originalHealth;
    private PlayerMovementControllerNoNetwork MovementController;
    public GameManagerScriptNoNetwork gameManager;
    private PhotonView PV;

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

        if (gameObject.tag == "Player")
        {
            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
        }
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
            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
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
        DodgeballScript dodgeballScript = collision.gameObject.GetComponent<DodgeballScript>();
        if (dodgeballScript != null)
        {
            TakeDamage(dodgeballScript.damage);
            dodgeballScript.MovementController.StartCoroutine(dodgeballScript.MovementController.Hitmarker());
            Destroy(collision.gameObject);
        }
    }

    //Destroys game object
    void Die()
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
    }

}
