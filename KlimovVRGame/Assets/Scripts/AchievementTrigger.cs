using UnityEngine;
using TMPro; // Добавляем пространство имен для TextMeshPro

public class AchievementTriggerTMP : MonoBehaviour
{
    public GameObject achievementPanel; // Ссылка на панель
    public TMP_Text achievementText; // Теперь используем TMP_Text вместо Text
    public string achievementName = "Новое достижение!";
    public float displayTime = 3f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            ShowAchievement();
        }
    }

    void ShowAchievement()
    {
        achievementText.text = achievementName;
        achievementPanel.SetActive(true);
        Invoke("HideAchievement", displayTime);
    }

    void HideAchievement()
    {
        achievementPanel.SetActive(false);
    }
}