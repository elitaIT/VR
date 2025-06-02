using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public bool destroyOnDeath = true;
    [Space(10)]

    [Header("Death Effects")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSound;
    public float deathSoundVolume = 1f;
    [Space(10)]

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onDamageTaken;

    private float currentHealth;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        // Автоматически добавляем AudioSource если нужен звук, но его нет
        if (deathSound != null && GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return; // Уже мертв

        currentHealth -= amount;
        onDamageTaken.Invoke();

        Debug.Log($"{gameObject.name} took {amount} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Воспроизводим звук смерти
        if (deathSound != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(deathSound, deathSoundVolume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            }
        }

        // Создаем эффект смерти
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        }

        // Вызываем событие смерти
        onDeath.Invoke();

        // Уничтожаем или деактивируем объект
        if (destroyOnDeath)
        {
            Destroy(gameObject, deathSound != null ? deathSound.length : 0f); // Ждем окончания звука
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"{gameObject.name} died!");
    }

    // Для отладки (опционально)
    void OnGUI()
    {
        if (Camera.main != null && GetComponent<Renderer>() != null && GetComponent<Renderer>().isVisible)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(screenPos.x - 50, Screen.height - screenPos.y - 50, 100, 30),
                     $"HP: {currentHealth}/{maxHealth}");
        }
    }
}