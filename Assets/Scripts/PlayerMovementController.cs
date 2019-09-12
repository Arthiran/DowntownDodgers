using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    //Get Character Properties
    private Transform Player;
    private CharacterController CharController;
    private Camera PlayerCam;

    //Initialize Variables
    private float velocityY;
    private Vector3 currentImpact;
    public float moveSpeed = 5f;
    public float mass = 1f;
    public float damping = 5f;
    public float edgeUpForce = 5f;
    public float climbSpeed = 100f;
    public float jumpForce = 4f;
    private float gravity = Physics.gravity.y;
    private float distWall = 1f;
    private bool jumping = false;
    private bool isClimbing = false;

    private void Start()
    {
        //Set Components to Variables at Start of Script
        Player = GetComponent<Transform>();
        CharController = GetComponent<CharacterController>();
        PlayerCam = GetComponentInChildren<Camera>();
    }

    //All Physics and Movement should be handled in FixedUpdate()
    private void FixedUpdate()
    {
        //Get Input from Horizontal and Vertical Axis and store them in variables
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //Create a Vector to store the overall movement
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        /*Takes the movement vector and converts the position from Local Space to World Space and stores 
        it back in the movement variable*/
        movement = transform.TransformDirection(movement);

        //Checks if Player is on the ground, if true set Y Velocity to 0
        if (CharController.isGrounded && velocityY < 0f)
        {
            velocityY = 0f;
        }

        //Calculates gravity and stores it in variable
        velocityY += gravity * Time.deltaTime;

        //Vector which stores the overall effect of gravity on the Character's position
        Vector3 velocity = movement * moveSpeed + Vector3.up * velocityY;

        //sets the velocity to take all forces into account
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        //Takes the velocity and actually moves the Character
        CharController.Move(velocity * Time.deltaTime);

        //Interpolates the effects of forces for smooth movement
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);

        /*This is so that when you press W and A at the same time for instance, the player doesn't become faster,
        it remains the same speed*/
        if (vertical != 0 && horizontal != 0)
        {
            vertical *= 0.7071f;
            horizontal *= 0.7071f;
        }

        //Jump pls
        Jump();
    }

    //Resets all forces
    private void ResetImpact()
    {
        currentImpact = Vector3.zero;
        velocityY = 0f;
    }

    //Resets forces on the Y axis
    private void ResetImpactY()
    {
        currentImpact.y = 0f;
        velocityY = 0f;
    }

    //Jump Function
    private void Jump()
    {
        //Checks if Space was pressed
        if (Input.GetButtonDown("Jump"))
        {
            //Performs a raycast that travels in the direction the character is facing
            RaycastHit hit;
            //This returns true if the distance to the wall is less than or equal to distWall
            if (Physics.Raycast(transform.position, transform.forward, out hit, distWall))
            {
                //Checks if the wall infront has the Climable script
                if (hit.collider.GetComponent<Climable>() != null)
                {
                    //Performs climbing mechanic Coroutine
                    StartCoroutine(Climb(hit.collider));
                    return;
                }
            }

            //Checks if player is on the ground, if true then character can jump
            if (CharController.isGrounded)
            {
                //Custom AddForce function which applies jumpForce in the upward direction
                AddForce(Vector3.up, jumpForce);
            }
        }
    }

    //Custom AddForce function, not to be mistakened with Rigidbody.AddForce()
    public void AddForce(Vector3 direction, float magnitude)
    {
        //Adds the force to the current amount of forces being applied to the game object
        currentImpact += direction.normalized * magnitude / mass;
    }

    //Climb Mechanic
    private IEnumerator Climb(Collider climableCollider)
    {
        isClimbing = true;
        while (Input.GetKey(KeyCode.Space))
        {
            //Performs a raycast that travels in the direction the character is facing(Same as before)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distWall))
            {
                //Checks if the wall is the same wall that was checked when Climb Coroutine was started
                if (hit.collider == climableCollider)
                {
                    //Gives the Character movement on the Y axis to climb up the wall
                    CharController.Move(new Vector3(0f, climbSpeed * Time.deltaTime, 0f));
                    yield return null;
                }
                //If it isn't, break
                else
                {
                    break;
                }
            }
            //If there is no wall directly infront anymore, break
            else
            {
                break;
            }
        }
        //Resets Forces on the Y axis
        ResetImpactY();
        //This is to nudge the character above the ledge of the top of the wall
        AddForce(Vector3.up, edgeUpForce);
        isClimbing = false;
    }
}
