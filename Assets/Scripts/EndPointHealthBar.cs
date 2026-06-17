using UnityEngine;
using UnityEngine.UI;

public class EndPointHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider healthSlider;


    private void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogWarning("EndPointHealthBar 没有绑定 Slider", this);
            return;
        }

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }

        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (GameManager.Instance == null || healthSlider == null)
            return;
        if (healthSlider != null)
        {
            healthSlider.value = (float) GameManager.Instance.lives / GameManager.Instance.maxLives;
        }
    }
}