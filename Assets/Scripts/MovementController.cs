using UnityEngine;

public class MovementController : MonoBehaviour, ITeleportable
{
    // Camera and rotation
    [Header("Camera")] [SerializeField] private GameObject pitchController;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float maxpitch, minpitch;

    // Movement
    [Header("WalkAndRun")] [SerializeField]
    private float speed = 10;

    [SerializeField] private float runningMultiplier = 2;
    [SerializeField] private bool canControlMovementMidAir;

    // Ground Check
    [Header("GroundCheck")] [SerializeField]
    private GameObject groundCheckPoint;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius;
    public bool isGrounded;

    // Jump and Gravity
    [Header("JumpAndGravity")] [SerializeField]
    private float gravity = 9.8f;

    [SerializeField] private float terminalVelocity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private bool doubleGravityWhenFalling;
    
    // Components
    private CharacterController characterController;
    private Vector3 horizontalMovement;
    private InputController input;
    private float jumpBufferTimer;

    private float verticalVelocity;
    private float yaw, pitch;

    private bool isTeleporting;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        input = GetComponent<InputController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        yaw = characterController.transform.rotation.eulerAngles.y;
        pitch = pitchController.transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        Rotation();
        CheckGround();
        JumpAndFall();
        Move();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.transform.position, checkRadius, groundLayer);
    }

    private void Rotation()
    {
        var breathingAmount = 1f;
        var breathingSpeed = .5f;
        var offsetX = Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;
        var offsetY = Mathf.Cos(Time.time * breathingSpeed) * breathingAmount;

        yaw += input.look.x * cameraSpeed * Time.deltaTime;
        pitch -= input.look.y * cameraSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minpitch, maxpitch);
        pitchController.transform.localRotation = Quaternion.Euler(offsetX + pitch, 0, 0);
        characterController.transform.rotation = Quaternion.Euler(0, offsetY + yaw, 0);
    }

    private void Move()
    {
        if (isGrounded || canControlMovementMidAir)
        {
            var inputDirection = new Vector3(input.move.x, 0, input.move.y);
            if (inputDirection.magnitude > 1) inputDirection = inputDirection.normalized;
            horizontalMovement = inputDirection * speed;

            //Corre solo hacia delante
            if (input.run && horizontalMovement.z > 0) horizontalMovement *= runningMultiplier;
            else input.run = false;
        }

        characterController.Move(transform.rotation * horizontalMovement * Time.deltaTime +
                                 new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void JumpAndFall()
    {
        //JumpBuffer
        if (input.jump)
        {
            jumpBufferTimer = jumpBufferTime;
            input.jump = false;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (isGrounded)
        {
            if (verticalVelocity < 0) verticalVelocity = -2f;
            //Salto
            if (jumpBufferTimer > 0)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                jumpBufferTimer = 0;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (doubleGravityWhenFalling && verticalVelocity < 0) verticalVelocity -= gravity * Time.deltaTime;
            verticalVelocity = Mathf.Min(verticalVelocity, terminalVelocity);
        }
    }

    public void Teleport(Portal portal)
    {
        if (isTeleporting) return;

        characterController.enabled = false;
        isTeleporting = true;

        Vector3 position = portal.virtualPortal.InverseTransformPoint(transform.position);
        Vector3 direction = portal.virtualPortal.InverseTransformDirection(transform.forward);

        transform.position = portal.otherPortal.transform.TransformPoint(position);
        transform.forward = portal.otherPortal.transform.TransformDirection(direction);

        yaw = transform.rotation.eulerAngles.y;

        characterController.enabled = true;
    }

    public void EndTeleport()
    {
        isTeleporting = false;
    }
}