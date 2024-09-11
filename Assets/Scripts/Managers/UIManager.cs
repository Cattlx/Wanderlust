using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void UpdateHealth(float healthPercent)
    {
        // Assuming healthPercent is a value between 0 and 1 (normalized)
        healthSlider.value = healthPercent;
    }
}
