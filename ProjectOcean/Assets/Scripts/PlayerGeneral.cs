using TMPro;
using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxThirst = 100f;
    [SerializeField] private float healthDecreaseRate = 1f;
    [SerializeField] private float staminaDecreaseRate = 2f;
    [SerializeField] private float hungerDecreaseRate = 1f;
    [SerializeField] private float thirstDecreaseRate = 1.5f;

    private float currentHealth;
    private float currentStamina;
    private float currentHunger;
    private float currentThirst;

    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;
    public float CurrentHunger => currentHunger;
    public float CurrentThirst => currentThirst;

    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;
    public float MaxHunger => maxHunger;
    public float MaxThirst => maxThirst;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI thirstText;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    private void Update()
    {
        DecreaseStats();
        CheckConditions();
    }

    private void LateUpdate()
    {
        UpdateUI();
    }

    private void DecreaseStats()
    {
        currentStamina -= staminaDecreaseRate * Time.deltaTime;
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    private void CheckConditions()
    {
        if((currentHunger <= 0 && currentThirst > 0) || (currentThirst <= 0 && currentHunger > 0))
        {
            currentHealth -= healthDecreaseRate * Time.deltaTime;
        }
        else if(currentHunger <= 0 && currentThirst <= 0)
        {
            currentHealth -= healthDecreaseRate * 2f * Time.deltaTime;
        }
    }

    private void UpdateUI()
    {
        healthText.text = $"Health: {currentHealth:F1}/{maxHealth}";
        staminaText.text = $"Stamina: {currentStamina:F1}/{maxStamina}";
        hungerText.text = $"Hunger: {currentHunger:F1}/{maxHunger}";
        thirstText.text = $"Thirst: {currentThirst:F1}/{maxThirst}";
    }

    private void Die()
    {
        // Oyuncu ölme işlemleri burada yapılacak
        Debug.Log("died");
    }

    #region Public Methods

    // --- DECREASE METHODS ---
    public void DecreaseHealth(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void DecreaseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public void DecreaseHunger(float amount)
    {
        currentHunger -= amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    public void DecreaseThirst(float amount)
    {
        currentThirst -= amount;
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }
    
    // --- RESTORE METHODS ---
    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public void RestoreHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    public void RestoreThirst(float amount)
    {
        currentThirst += amount;
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    #endregion
}
