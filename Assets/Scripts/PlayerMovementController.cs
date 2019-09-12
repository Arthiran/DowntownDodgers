using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    private Transform Player;
    //public Animator Animator;
    private Rigidbody RigidBody;
    private CharacterController CharController;
    private CapsuleCollider capsuleCollider;
    private Camera PlayerCam;

    public float moveSpeed = 35; // move speed
    public float edgeUpForce = 5;
    public float climbSpeed = 100;
    public float jumpForce = 4; // vertical jump initial speed
    private float gravity = 100; // gravity acceleration
    //private float runSpeed = 65; // move speed
    //public float gravity = 50; // gravity acceleration
    private float deltaGround = 0.2f; // character is grounded up to this distance
    private float distGround; // distance from character position to ground
    private float distWall = 1;
    private bool jumping = false;
    private bool isClimbing = false;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Player = GetComponent<Transform>();
        RigidBody = GetComponent<Rigidbody>();
        CharController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        PlayerCam = GetComponentInChildren<Camera>();
        RigidBody.freezeRotation = true;
        distGround = capsuleCollider.height - capsuleCollider.center.y;

    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);

        if (vertical != 0 && horizontal != 0)
        {
            vertical *= 0.7071f;
            horizontal *= 0.7071f;
        }

        Jump();
        RigidBody.MovePosition(transform.position + (transform.forward * movement.z) + (transform.right * movement.x));
        RigidBody.AddForce(-gravity * RigidBody.mass * transform.up);
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
            if (isGrounded())
            {
                RigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(Player.position, -Vector3.up, distGround + deltaGround);
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
                    Vector3 movement = new Vector3(0f, climbSpeed * Time.deltaTime, 0f);
                    Debug.Log(movement.y);
                    RigidBody.MovePosition(transform.position + (transform.up * movement.y));
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
        RigidBody.AddForce(Vector3.up * edgeUpForce);
        isClimbing = false;
    }
}
