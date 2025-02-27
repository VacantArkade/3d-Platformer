using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpForce = 1;
    [SerializeField] float gravity = 1;

    [SerializeField] Transform myCamera;

    [SerializeField] Animator myAnimator;

    CharacterController controller;

    Rigidbody rb;

    Vector3 movement;

    bool grounded;

    [SerializeField] PlayerStats myStats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = myStats.moveSpeed;
        jumpForce = myStats.jumpForce;
        gravity = myStats.gravity;
        myStats.health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (xInput != 0 || yInput != 0)
        {
            myAnimator.SetBool("isJogging", true);
        }
        else
        {
            myAnimator.SetBool("isJogging", false);
        }

        Vector3 camForward = myCamera.forward;
        Vector3 camRight = myCamera.right;

        camForward.y = 0;
        camForward.Normalize();

        camRight.y = 0;
        camRight.Normalize();

        Vector3 forwardRelativeMovementVector = yInput * camForward;
        Vector3 rightRelativeMovementVector = xInput * camRight;

        var relativeMovement = (forwardRelativeMovementVector + rightRelativeMovementVector) * speed;

        if (xInput != 0 || yInput != 0)
        {
            transform.forward = relativeMovement;
        }

        relativeMovement.y = movement.y;
        movement = relativeMovement;

        movement.y += gravity * Time.deltaTime;



        if (controller.isGrounded)
        {
            movement.y = 0;
        }

        grounded = Physics.Raycast(transform.position + Vector3.down, Vector3.down, 1);

        myAnimator.SetBool("onGround", grounded);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            movement.y = jumpForce;
            myAnimator.SetTrigger("jump");
        }

        controller.Move(movement * Time.deltaTime);

        if (myStats.health == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hurt")
        {
            hurt();
        }

        if (other.gameObject.tag == "Kill")
        {
            SceneManager.LoadScene("GameOver");
        }

        if (other.gameObject.tag == "Win")
        {
            SceneManager.LoadScene("Victory");
        }
    }

    private void hurt()
    {
        myStats.health -= 1;
    }
}
