using UnityEngine;

public class Target : MonoBehaviour {

    //Initialize Variables
    public float health = 30f;
    public float originalHealth;
    private PlayerMovementController MovementController;

    private void Start()
    {
        originalHealth = health;
        MovementController = GetComponent<PlayerMovementController>();
        if (gameObject.tag == "Player")
        {
            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
        }
    }

    //Calculates the amount of damage taken from shot, dies if under 0 health
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (gameObject.tag == "Player")
        {
            MovementController.HealthNumText.text = health.ToString() + "/" + originalHealth.ToString();
            MovementController.filledHealthbarIMG.fillAmount = health / originalHealth;
        }
        if(health <= 0f)
        {
            Die();
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
