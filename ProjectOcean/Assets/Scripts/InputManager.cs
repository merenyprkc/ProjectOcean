using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions inputActions;

    public event Action OnInteractPressed;
    public event Action OnJumpPressed;
    public event Action OnInventoryToggle;

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
        inputActions.Player.Screenshot.performed += ctx => GetScreenshot();
        inputActions.Player.Inventory.performed += ctx => OpenCloseInventory();
    }

    private void Update()
    {
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = inputActions.Player.Look.ReadValue<Vector2>();   
        IsSprinting = inputActions.Player.Sprint.IsPressed();
    }

    private void GetScreenshot()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filename = $"Screenshot_{timestamp}.png";
        ScreenCapture.CaptureScreenshot(filename);
        Debug.Log($"Screenshot taken: {filename}");
    }

    private void OpenCloseInventory()
    {
        OnInventoryToggle?.Invoke();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    public void EnablePlayerControls()
    {
        inputActions.Player.Move.Enable();
        inputActions.Player.Look.Enable();
        inputActions.Player.Sprint.Enable();
        inputActions.Player.Jump.Enable();
        inputActions.Player.Interact.Enable();
    }

    public void DisablePlayerControls()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.Look.Disable();
        inputActions.Player.Sprint.Disable();
        inputActions.Player.Jump.Disable();
        inputActions.Player.Interact.Disable();
    }
}
