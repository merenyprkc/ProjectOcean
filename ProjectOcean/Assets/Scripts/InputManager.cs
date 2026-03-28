using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions inputActions;

    public event Action OnInteractPressed;
    public event Action OnJumpPressed;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsSprinting { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        inputActions = new InputSystem_Actions();

        inputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
        inputActions.Player.Jump.performed += ctx => OnJumpPressed?.Invoke();
    }

    private void Update()
    {
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = inputActions.Player.Look.ReadValue<Vector2>();   
        IsSprinting = inputActions.Player.Sprint.IsPressed();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public void EnablePlayerControls() => inputActions.Player.Enable();
    public void DisablePlayerControls() => inputActions.Player.Disable();
}
