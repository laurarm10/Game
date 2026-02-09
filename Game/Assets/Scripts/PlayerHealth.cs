using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;
    public TMP_Text livesText;

    public float damageCooldown = 1f; // 1 segundo entre daños
    private float damageTimer = 0f;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }
    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (damageTimer > 0) return; // Espera cooldown

        currentLives -= amount;
        if (currentLives < 0) currentLives = 0;

        UpdateLivesUI();
        damageTimer = damageCooldown;

        if (currentLives == 0)
        {
            Debug.Log("GAME OVER");
        }
    }

    void UpdateLivesUI()
    {
        livesText.text = "Vidas: " + currentLives;
    }
}