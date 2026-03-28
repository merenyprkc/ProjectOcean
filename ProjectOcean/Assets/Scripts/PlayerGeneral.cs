using TMPro;
using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    [Header("Settings")]
    // --- MAX VALUES ---
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxThirst = 100f;
    // --- DECREASE RATES ---
    [SerializeField] private float healthDecreaseRate = 1f;
    [SerializeField] private float staminaDecreaseRate = 2f;
    [SerializeField] private float hungerDecreaseRate = 1f;
    [SerializeField] private float thirstDecreaseRate = 1.5f;
    // --- RESTORE RATES ---
    [SerializeField] private float healthRestoreRate = 5f;
    [SerializeField] private float staminaRestoreRate = 10f;
    // --- DELAYS ---
    [SerializeField] private float staminaRestoreDelay = 3f;

    private float lastStaminaUseTime;
    private float cachedHealth, cachedStamina, cachedHunger, cachedThirst;

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
        DecreaseStatsOvertime();
        CheckConditions();
        RestoreStaminaOvertime();
    }

    private void LateUpdate()
    {
        UpdateUI();
    }

    private void DecreaseStatsOvertime()
    {
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;

        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    private void RestoreStaminaOvertime()
    {
        if (currentStamina < maxStamina && Time.time - lastStaminaUseTime >= staminaRestoreDelay)
        {
            currentStamina += staminaRestoreRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    private void CheckConditions()
    {
        bool hungerEmpty = currentHunger <= 0;
        bool thirstEmpty = currentThirst <= 0;

        if (hungerEmpty && thirstEmpty)
            currentHealth -= healthDecreaseRate * 2f * Time.deltaTime;
        else if (hungerEmpty || thirstEmpty)
            currentHealth -= healthDecreaseRate * Time.deltaTime;

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    private void UpdateUI()
    {
        if (!Mathf.Approximately(cachedHealth, currentHealth))
        {
            cachedHealth = currentHealth;
            healthText.text = $"Health: {currentHealth:F1}/{maxHealth}";
        }
        if (!Mathf.Approximately(cachedStamina, currentStamina))
        {
            cachedStamina = currentStamina;
            staminaText.text = $"Stamina: {currentStamina:F1}/{maxStamina}";
        }
        if (!Mathf.Approximately(cachedHunger, currentHunger))
        {
            cachedHunger = currentHunger;
            hungerText.text = $"Hunger: {currentHunger:F1}/{maxHunger}";
        }
        if (!Mathf.Approximately(cachedThirst, currentThirst))
        {
            cachedThirst = currentThirst;
            thirstText.text = $"Thirst: {currentThirst:F1}/{maxThirst}";
        }
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
        lastStaminaUseTime = Time.time;
    }

    public void DecreaseStamina()
    {
        currentStamina -= staminaDecreaseRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        lastStaminaUseTime = Time.time;
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
