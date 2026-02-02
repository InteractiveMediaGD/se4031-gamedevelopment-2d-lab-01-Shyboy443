using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public TMP_Text healthText;
    public TMP_Text gameOverText;
    
    private float damageCooldown = 1f; // Prevent instant multiple damage
    private float lastDamageTime = -999f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
        
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        // Cooldown to prevent instant death
        if (Time.time - lastDamageTime < damageCooldown)
            return;
            
        lastDamageTime = Time.time;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER!\nPress R to Restart";
        }
    }

    void Update()
    {
        // Restart with R key when game is over
        if (currentHealth == 0 && UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetHealth();
            
            // Reset player position
            transform.position = new Vector3(-3f, 0f, 0f);
            
            // Reset score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.ResetScore();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1f;
        UpdateUI();
        
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TakeDamage(10);
        }
        else if (other.CompareTag("Collectible"))
        {
            // Collect item for score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.AddScore(10);
            
            Destroy(other.gameObject);
        }
    }
}
