using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    private bool inputLocked = false;

    public bool IsInputLocked() => inputLocked;


    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        if (inputLocked) return Vector2.zero;
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped()
    {
        return playerControls.Player.Jump.triggered;
    }
    
    public void DisablePlayerInput()
    {
        inputLocked = true;
        playerControls.Disable();
    }

    public void EnablePlayerInput()
    {
        inputLocked = false;
        playerControls.Enable();
    }


}
