using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Player playerData;
    public Slider healthSlider;
    public Slider steminaSlider;

    private float targetHealth;   // 실제 따라가야 할 값
    private float targetStemina;
    public float smoothSpeed = 5f; // 슬라이더 줄어드는 스피드, 높을수록 빠름
    // Start is called before the first frame update
    void Start()
    {
        targetHealth = playerData.currentHP / playerData.maxHP;
        targetStemina = playerData.currentStamina / playerData.maxStamina; 
        
        healthSlider.value = targetHealth;
        steminaSlider.value = targetStemina;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
        UpdateSteminaUI();
    }

    private void UpdateHealthUI()
    {
        float current = playerData.currentHP / playerData.maxHP;

        targetHealth = current;

        healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealth, Time.deltaTime * smoothSpeed);
    }
    private void UpdateSteminaUI()
    {
        float current = playerData.currentStamina / playerData.maxStamina;

        targetStemina = current;

        steminaSlider.value = Mathf.Lerp(steminaSlider.value, targetStemina, Time.deltaTime * smoothSpeed);
    }
}
