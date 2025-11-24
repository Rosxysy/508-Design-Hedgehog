using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rolling : MonoBehaviour
{
    [Header("Rolling Settings")]
    [SerializeField, Range(0f, 30f)] float moveSpeed = 1f;
    [SerializeField, Range(0f, 10f)] float acceleration = 5f;

    [SerializeField, Range(0f, 1f)] float rollSpeedMultiplier = 0.5f;

     private Vector3 smoothedVelocity = Vector3.zero;

    [SerializeField] Transform visualModel; // The mesh that visually rotates
    [SerializeField] Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private float radius = 0.5f; // sphere radius for rotation speed

    private bool rollingEnabled = false;
    public bool IsRolling => rollingEnabled;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void SetRolling(bool enable)
    {
        rollingEnabled = enable;
        if (!enable)
            rb.linearVelocity = Vector3.zero;
    }

    
    public void Roll(Vector3 rbVelocity)
    {
    if (!rollingEnabled || rbVelocity.sqrMagnitude < 0.01f)
            return;
        
    smoothedVelocity = Vector3.Lerp(smoothedVelocity, rbVelocity, acceleration * Time.fixedDeltaTime);

    if (visualModel != null)
    {
        float distance = rbVelocity.magnitude * Time.fixedDeltaTime;
        float rotationAngle = (distance / radius) * Mathf.Rad2Deg * rollSpeedMultiplier;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, rbVelocity.normalized);
        visualModel.Rotate(rotationAxis, rotationAngle, Space.World);
    }
    }

}