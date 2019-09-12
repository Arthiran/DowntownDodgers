using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerMovement : MonoBehaviour
{

    private Transform Player;
    private CharacterController CharController;
    private Camera PlayerCam;

    private float velocityY;
    private Vector3 currentImpact;
    public float moveSpeed = 5f; // move speed
    public float mass = 1f;
    public float damping = 5f;
    public float edgeUpForce = 5f;
    public float climbSpeed = 100f;
    public float jumpForce = 4f; // vertical jump initial speed
    private float gravity = Physics.gravity.y; // gravity acceleration
    private float distWall = 1f;
    private bool jumping = false;
    private bool isClimbing = false;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Player = GetComponent<Transform>();
        CharController = GetComponent<CharacterController>();
        PlayerCam = GetComponentInChildren<Camera>();

    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        movement = transform.TransformDirection(movement);

        if (CharController.isGrounded && velocityY < 0f)
        {
            velocityY = 0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = movement * moveSpeed + Vector3.up * velocityY; 

        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        CharController.Move(velocity * Time.deltaTime);
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);

        if (vertical != 0 && horizontal != 0)
        {
            vertical *= 0.7071f;
            horizontal *= 0.7071f;
        }

        Jump();
    }

    private void ResetImpact()
    {
        currentImpact = Vector3.zero;
        velocityY = 0f;
    }

    private void ResetImpactY()
    {
        currentImpact.y = 0f;
        velocityY = 0f;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distWall))
            {
                if (hit.collider.GetComponent<Climable>() != null)
                {
                    StartCoroutine(Climb(hit.collider));
                    return;
                }
            }

            if (CharController.isGrounded)
            {
                AddForce(Vector3.up, jumpForce);
            }
        }
    }

    public void AddForce(Vector3 direction, float magnitude)
    {
        currentImpact += direction.normalized * magnitude / mass;
    }

    private IEnumerator Climb(Collider climableCollider)
    {
        isClimbing = true;
        while (Input.GetKey(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distWall))
            {
                if (hit.collider == climableCollider)
                {
                    CharController.Move(new Vector3(0f, climbSpeed * Time.deltaTime, 0f));
                    yield return null;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        ResetImpactY();
        AddForce(Vector3.up, edgeUpForce);
        isClimbing = false;
    }
}
