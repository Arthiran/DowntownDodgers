using UnityEngine;

public class Target : MonoBehaviour {

    //Initialize Variables
    public float health = 30f;

    //Calculates the amount of damage taken from shot, dies if under 0 health
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    //Destroys game object
    void Die()
    {
        Destroy(gameObject);
    }

}
