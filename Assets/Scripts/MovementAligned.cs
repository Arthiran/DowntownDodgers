using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovementAligned : MonoBehaviour
{

    public Transform Player;
    public Rigidbody rigidbody;
    public Animator Animator;
    public Animator Crosshair;

    private float moveSpeed = 35; // move speed
    private float runSpeed = 65; // move speed
    private float turnSpeed = 90; // turning speed (degrees/second)
    private float lerpSpeed = 10; // smoothing speed
    private float gravity = 100; // gravity acceleration
    private bool isGrounded;
    private bool isGrounded2;
    private float deltaGround = 0.2f; // character is grounded up to this distance
    private float jumpSpeed = 40; // vertical jump initial speed
    private float jumpRange = 1; // range to detect target wall
    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal
    private float distGround; // distance from character position to ground
    private bool jumping = false; // flag &quot;I'm jumping to wall&quot;
    private float vertSpeed = 0; // vertical jump current speed

    private Transform myTransform;
    //public BoxCollider boxCollider; // drag BoxCollider ref in editor
    public CapsuleCollider capsuleCollider;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        myNormal = transform.up; // normal starts as character up direction
        myTransform = transform;
        rigidbody.freezeRotation = true; // disable physics rotation
                                         // distance from transform.position to ground
        //distGround = boxCollider.extents.y - boxCollider.center.y;
        distGround = capsuleCollider.height - capsuleCollider.center.y;

    }

    private void FixedUpdate()
    {
        // apply constant weight force according to character normal:
        rigidbody.AddForce(-gravity * rigidbody.mass * myNormal);
    }

    void Update()
    {
        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall

        Ray ray;
        RaycastHit hit;

       if (Input.GetButtonDown("Jump"))
        { // jump pressed:
            ray = new Ray(myTransform.position, myTransform.forward);
            /*if (Physics.Raycast(ray, out hit, jumpRange))
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
            }*/
            if (isGrounded)
            { // no: if grounded, jump up
                Animator.SetTrigger("Jump");
                rigidbody.velocity += jumpSpeed * myNormal;
            }
            else if (isGrounded2)
            { // no: if grounded2, jump up
                Animator.SetTrigger("Jump");
                rigidbody.velocity += jumpSpeed * myNormal;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Animator.SetInteger("Scoped", 1);
            Crosshair.SetInteger("Scoped", 1);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Animator.SetInteger("Scoped", 0);
            Crosshair.SetInteger("Scoped", 0);
        }

        // movement code - turn left/right with Horizontal axis:

        // update surface normal and isGrounded:
        ray = new Ray(myTransform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character forth/back with Vertical axis:
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);
    
        Animator.SetFloat("Horizontal", horizontal * 1, 40f, Time.deltaTime * 100f);
        Animator.SetFloat("Vertical", vertical * 1, 40f, Time.deltaTime * 100f);

        if (vertical != 0 && horizontal != 0)
        {
            vertical *= 0.7071f;
            horizontal *= 0.7071f;
        }

        rigidbody.MovePosition(transform.position + (transform.forward * movement.z) + (transform.right * movement.x));
    }

    void OnGUI()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Event e = Event.current;
        if (Input.GetButton("Run") && e.shift)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed * Time.deltaTime, 0, vertical * runSpeed * Time.deltaTime);

            if (vertical != 0 && horizontal != 0)
            {
                vertical *= 0.7071f;
                horizontal *= 0.7071f;
            }

            rigidbody.MovePosition(transform.position + (transform.forward * movement.z) + (transform.right * movement.x));
        }
    }

    void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumping = true; // signal it's jumping to wall
        rigidbody.isKinematic = true; // disable physics while jumping
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
        }
        myNormal = normal; // update myNormal
        rigidbody.isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Animator.SetInteger("OnObstacle2", 1);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Animator.SetInteger("OnObstacle2", 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded2 = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded2 = false;
        }
    }
}
