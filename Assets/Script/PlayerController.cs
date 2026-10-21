using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private Vector3 moveVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(moveX, 0f, moveZ).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            moveVelocity = moveDir * moveSpeed;

            if (animator != null) animator.SetBool("IsMoving", true);
        }
        else
        {
            moveVelocity = Vector3.zero;
            if (animator != null) animator.SetBool("IsMoving", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            if (animator != null) 
            {
                animator.ResetTrigger("JumpRequested");
                animator.SetTrigger("JumpRequested");
            }
        }

        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = moveVelocity + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }
}