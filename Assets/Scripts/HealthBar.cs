using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider FillSlider;

    public void SetHealth(int health, int maxHealth)
    {
        FillSlider.value = (float)health/maxHealth;
    }
}
