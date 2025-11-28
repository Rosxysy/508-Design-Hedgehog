using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    public PlayerEnergy playerEnergy;   // reference to your energy script
    public Slider energySlider;

    void Start()
    {
        // Initialize slider
        energySlider.maxValue = playerEnergy.maxEnergy;
        energySlider.value = playerEnergy.currentEnergy;
    }

    void Update()
    {
        // Update slider every frame
        energySlider.value = playerEnergy.currentEnergy;
    }
}
