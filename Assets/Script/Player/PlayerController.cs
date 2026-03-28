using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{

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

    [Header("Camera Settings")]
    public CinemachineCamera virtualCamera;
    private CinemachineInputAxisController camInput;
    private CinemachineOrbitalFollow camOrbitFollow;
    private Transform camTransform;
    public float mouseSensitivity = 2f; // 마우스 감도
    [Header("Handle Settings")]
    public float moveSpeed = 5f;
    private const float moveOriginalSpeed = 5f;
    private const float moveSprintSpeed = 8f;
    public float rotationSpeed = 15f; // 캐릭터 몸 회전 속도

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float groundCheckRadius = 0.01f; // 발밑 체크 구체의 반지름
    public LayerMask groundLayer;           // 바닥으로 인식할 레이어 (Inspector에서 설정)

    private Animator anim;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        movement = GetComponent<Movement>();
        //anim = GetComponent<Animator>();
        camTransform = Camera.main.transform;
        if (virtualCamera != null)
        {
            // 최신 버전의 입력 제어 컴포넌트
            camInput = virtualCamera.GetComponent<CinemachineInputAxisController>();
            camOrbitFollow = virtualCamera.GetComponent<CinemachineOrbitalFollow>();
        }
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        camInput.enabled = true;
        HandleMovementAndRotation(); // 이동 및 회전
    }

    void HandleMovementAndRotation()
    {
        bool isRightClick = Input.GetMouseButton(1);

        camOrbitFollow.HorizontalAxis.Recentering.Enabled = !isRightClick;
        camOrbitFollow.VerticalAxis.Recentering.Enabled = !isRightClick;
        camOrbitFollow.RadialAxis.Recentering.Enabled = !isRightClick;

        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        anim.SetFloat("Blend", inputDir.magnitude);
        if (inputDir.magnitude >= 0.1f)
        {
            // [핵심] 현재 카메라가 바라보고 있는 방향 벡터를 그대로 가져옵니다.
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // Y축(높이)은 무시하고 수평 방향만 남깁니다.
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 마우스로 정해놓은 targetYaw를 기준으로 '이동용 기준 좌표계'를 만듭니다.
            Vector3 moveDirection = (Vector3.forward * inputDir.z +
                                     Vector3.right * inputDir.x).normalized;

            // 우클릭 중에만 마우스 X축 움직임을 가져옴
            if (isRightClick)
            {
                moveDirection = (camForward * inputDir.z + camRight * inputDir.x).normalized;
            }

            // 1. 물리 이동 전달
            movement.MoveDirection = moveDirection;

            // 2. 캐릭터 회전 (이동 방향으로 몸을 돌림)
            // 단일 오브젝트이므로 transform.rotation을 직접 수정합니다.
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            
            isWalking = true;
        }
        else
        {
            movement.MoveDirection = Vector3.zero;
            isWalking = false;
        }
        anim.SetBool("IsGround", IsGrounded());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            moveLastInput = moveInput;
            moveDir = moveInput;
        }
        moveInput = context.ReadValue<Vector2>();
        if (!context.canceled) moveDir = moveInput;
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
            anim.SetTrigger("IsJump");
        }
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
            anim.speed = 2;
            moveSpeed = moveSprintSpeed;
        }
        if (context.canceled)
        {
            isSprint = false;
            anim.speed = 1;
            moveSpeed = moveOriginalSpeed;
            //anim.SetBool("IsWalking", false);
        }
    }
}
