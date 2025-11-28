using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    [Tooltip("Energy lost per meter traveled.")]
    public float energyPerMeter = 1f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        DrainEnergyByDistance();
    }

    void DrainEnergyByDistance()
    {
        // Distance moved this frame
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // Drain energy proportional to distance
        currentEnergy -= distanceMoved * energyPerMeter;

        // Clamp to valid range
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        // Update last position for next frame
        lastPosition = transform.position;
    }
}
