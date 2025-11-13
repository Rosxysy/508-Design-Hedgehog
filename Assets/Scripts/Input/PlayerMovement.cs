using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction;

    public float moveSpeed = 5f;
    private Rigidbody rb;

    Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; //smooth movement
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; //prevent falling over
    }

    void OnEnable()
    {
        moveAction?.action.Enable();

    }

    void OnDisable()
    {
        moveAction?.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero; 
    }

    void FixedUpdate() //every other frame
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();

        Vector3 move = transform.TransformDirection(inputDir) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
}

