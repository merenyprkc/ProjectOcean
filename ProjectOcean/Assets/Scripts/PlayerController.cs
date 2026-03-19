using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Grounded,
        InAir,
        Swimming
    }

    [Header("Player Settings")]
    private Rigidbody rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    public PlayerState currentState;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("Camera Settings")]
    private float xRotation = 0f;
    private Vector2 lookInput;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 10f;

    [Header("Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float checkRadius = 0.15f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();

        inputActions.Player.Jump.performed += ctx => Jump();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() => inputActions.Player.Enable();

    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        UpdatePlayerState();
        RotateCamera();
    }

    private void FixedUpdate()
    {
        switch(currentState)
        {
            case PlayerState.Grounded:
                MoveGrounded();
                break;
            case PlayerState.InAir:
                MoveInAir();
                break;
            case PlayerState.Swimming:
                // Yüzme hareketi gelecek
                break;
        }

        SpeedControl();
    }

    private void UpdatePlayerState()
    {
        if(IsGrounded())
        {
            currentState = PlayerState.Grounded;
        }
        else if(IsSwimming())
        {
            currentState = PlayerState.Swimming;
        }
        else
        {
            currentState = PlayerState.InAir;
        }
    }

    #region Movement Methods
    
    private void MoveGrounded()
    {
        Vector2 input = moveInput;
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        rb.linearDamping = groundDrag;
    }

    private void MoveInAir()
    {
        Vector2 input = moveInput;
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        
        rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        rb.linearDamping = 0f;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    #endregion

    #region Camera Methods

    public void RotateCamera()
    {
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    #endregion

    #region Checking Methods

    private bool IsGrounded()
    {
        if(Physics.CheckSphere(transform.position + new Vector3(0f, -1f, 0f), checkRadius, groundLayer))
        {
            return true;
        }
        return false;
    }

    private bool IsSwimming()
    {
        if(Physics.CheckSphere(transform.position, checkRadius, waterLayer))
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Control

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(flatVelocity.magnitude > speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    #endregion
}
