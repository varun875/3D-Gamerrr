using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Input actions
    InputAction moveAction;
    InputAction jumpaction;
    InputAction runAction; // Add run action

    // Player
    CharacterController controller; // component
    PlayerInput playerInput; // component

    // Move
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float runSpeed = 20F; // Add run speed

    // Jump
    Vector3 PlayerVelocity;
    [SerializeField] bool groundedPlayer;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;

    // Rotation
    [SerializeField] float rotationSpeed = 200f;

    // Animation
    private Animator animator;

    // Camera
    private Transform camTransform;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        moveAction = playerInput.actions["Move"];
        jumpaction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"]; // initialize run action

        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && PlayerVelocity.y < 0)
        {
            PlayerVelocity.y = 0f;
        }

        // Movement
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Camera-relative movement
        Vector3 forward = camTransform != null ? camTransform.forward : Vector3.forward;
        Vector3 right = camTransform != null ? camTransform.right : Vector3.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 move = forward * input.y + right * input.x;

        float currentSpeed = moveSpeed; // default is walk speed

        if (runAction.IsPressed()) // check for run action
        {
            currentSpeed = runSpeed;
        }

        controller.Move(move * Time.deltaTime * currentSpeed);

        if (input.magnitude > 0.1f)
        {
            RotateCharacter(move);
            animator.SetFloat("Speed", currentSpeed); // set animator speed
        }
        else
        {
            animator.SetFloat("Speed", 0f); // set idle animation
        }

        // Jump
        if (jumpaction.phase == InputActionPhase.Performed && groundedPlayer)
        {
            PlayerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        PlayerVelocity.y += gravity * Time.deltaTime;
        controller.Move(PlayerVelocity * Time.deltaTime);
    }

    private void RotateCharacter(Vector3 input)
    {
        if (input.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}