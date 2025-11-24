using UnityEngine;
using UnityEngine.EventSystems;

public class HedgehogControllerExpired : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f;

     [SerializeField] Rolling rollingMovement;

    private Rigidbody rb;
    private Vector3 currentVelocity;

    [SerializeField]
    public Transform cameraTransform;
    public Transform initialTransform;

    private bool rollingMode = false;
     

    void Awake()
    {
        rb = GetComponent<Rigidbody>();


        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialTransform = this.transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            rollingMode = !rollingMode;
            rollingMovement.SetRolling(rollingMode);
        }

    }
    void FixedUpdate()
    {
        Movement();
        LookAtCamera();

        if (rollingMode)
        {
            // Pass current horizontal velocity to the rolling script
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0f;
            rollingMovement.Roll(horizontalVelocity);
        }
    
    }

    private void Movement()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);


        if (rb == null || cameraTransform == null)
            return;


        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;


        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();


        Vector3 moveDir = camRight * playerInput.x + camForward * playerInput.y;
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        NormalMove(moveDir);
    
        if (rollingMode)
        {
            // Rolling handled by separate script
            rollingMovement.Roll(rb.linearVelocity);
        }
    
    }

    private void LookAtCamera() 
    {
        if (cameraTransform == null || rb == null)
            return;

        Vector3 lookDirection = cameraTransform.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
    }

    private void NormalMove(Vector3 moveDir)
    {
        Vector3 desiredVelocity = moveDir * maxSpeed;
        rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);
    }
}
