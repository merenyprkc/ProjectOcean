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

    private PlayerGeneral playerGeneral;

    [Header("Player Settings")]
    public PlayerState currentState;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airMultiplier = 0.4f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float yRotation = 0f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float sprintFOV = 80f;
    [SerializeField] private float normalFOV = 60f;
    private float xRotation = 0f;
    private Camera playerCamera;


    [Header("Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float checkRadius = 0.15f;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;

        playerCamera = cameraTransform.GetComponent<Camera>();

        yRotation = transform.eulerAngles.y;

        if (InputManager.Instance != null) InputManager.Instance.OnJumpPressed += Jump;
        else Debug.LogError("InputManager instance is null!");

        playerGeneral = GetComponent<PlayerGeneral>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnJumpPressed -= Jump;
        }
    }

    private void Update()
    {
        moveInput = InputManager.Instance.MoveInput;
        lookInput = InputManager.Instance.LookInput;

        UpdatePlayerState();
    }

    private void LateUpdate()
    {
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
        AnimationControl();
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

        Vector3 force = moveDirection.normalized * speed * 10f;

        if(InputManager.Instance.IsSprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, Time.deltaTime * 10f);
            force = moveDirection.normalized * sprintSpeed * 10f;
            playerGeneral.DecreaseStamina();
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);
        }
        
        rb.AddForce(force, ForceMode.Force);
        rb.linearDamping = groundDrag;
    }

    private void MoveInAir()
    {
        Vector2 input = moveInput;
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        Vector3 force = moveDirection.normalized * speed * 10f * airMultiplier;

        rb.AddForce(force, ForceMode.Force);
        rb.linearDamping = 0f;
    }

    private void Jump()
    {
        if(currentState == PlayerState.Grounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            playerGeneral.DecreaseStamina(7.5f);
        }
    }

    #endregion

    #region Rotation Methods

    public void RotateCamera()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // 1. Kamerayı Yukarı/Aşağı Eğme (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 2. Oyuncunun Gövdesini Sağa/Sola Döndürme (Yaw)
        yRotation += mouseX;
        rb.MoveRotation(Quaternion.Euler(0f, yRotation, 0f));
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

    #region Animations

    private void AnimationControl()
    {
        if(currentState == PlayerState.Grounded)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
        }
        else if(currentState == PlayerState.InAir)
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    #endregion
}
