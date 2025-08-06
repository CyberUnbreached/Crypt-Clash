using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;

    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;
    private bool canMove = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;

        if (inputManager == null)
        {
            Debug.LogError("InputManager.Instance is NULL. Is InputManager in the scene?");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;

        if (InputManager.Instance.IsInputLocked())
            return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Horizontal input
        Vector2 movement = inputManager.GetPlayerMovement();
        // Use camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * movement.x + camForward * movement.y;


        move = Vector3.ClampMagnitude(move, 1f); // Optional: prevents faster diagonal movement

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Jump
        if (inputManager.PlayerJumped() && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    
    public void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - cameraTransform.position;
        direction.y = 0f; // Optional: keep camera level
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        cameraTransform.rotation = targetRotation;
    }

}
