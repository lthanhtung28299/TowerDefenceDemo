using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRendererView : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    public void UpdateHealthView(float healthProgress)
    {
        healthFill.fillAmount = healthProgress;
    }
}
