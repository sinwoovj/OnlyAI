using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;

    [HideInInspector]
    public PlayerInput playerInput;
    private Movement movement;
    public Transform footTransform;

    [SerializeField]
    private Vector2 moveLastInput = Vector2.zero;
    [SerializeField]
    private Vector2 moveInput = Vector2.zero;
    public Vector2 moveDir = Vector2.zero;
    public bool isWalking = false;
    public bool isSprint = false;
    public bool isCrouch = false;
    public bool isJump = false;

    [Header("Look Settings")]
    public float mouseSensitivity = 0.5f; // 마우스 감도 (적절히 조절하세요)
    public Transform cameraTransform;     // 플레이어 하위에 있는 메인 카메라를 할당하세요
    public float camAngleLimitUp = -90f;
    public float camAngleLimitDown = 30f;
    private float xRotation = 0f;         // 상하 회전값을 누적해서 저장할 변수

    [Header("Jump Settings")]
    public float groundCheckRadius = 0.2f; // 발밑 체크 구체의 반지름
    public LayerMask groundLayer;           // 바닥으로 인식할 레이어 (Inspector에서 설정)

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        movement = GetComponent<Movement>();
        //anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void Update()
    {
        movement.MoveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        isWalking = true;
        //anim.SetBool("IsWalking", true);
        if (context.canceled)
        {
            isWalking = false;
            //anim.SetBool("IsWalking", false);
            //anim.SetFloat("LastInputX", moveInput.x);
            //anim.SetFloat("LastInputY", moveInput.y);
            moveLastInput = moveInput;
            moveDir = moveInput;
        }
        moveInput = context.ReadValue<Vector2>();
        if (!context.canceled) moveDir = moveInput;
        //anim.SetFloat("InputX", moveInput.x);
        //anim.SetFloat("InputY", moveInput.y);
    }
    private bool IsGrounded()
    {
        return Physics.CheckSphere(footTransform.position, groundCheckRadius, groundLayer);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            isJump = true;
            movement.Jump();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>(); 
        
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, camAngleLimitUp, camAngleLimitDown);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {

    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (isWalking)
        {
            isSprint = true;
        }
        if (context.canceled)
        {
            isSprint = false;
            //anim.SetBool("IsWalking", false);
        }
    }
}
