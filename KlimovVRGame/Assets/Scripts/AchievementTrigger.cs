using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AchievementTrigger : MonoBehaviour
{
    [Header("Настройки триггера")]
    public AchievementTriggerType triggerType;

    [Header("Параметры (зависит от типа)")]
    public int floorNumber; // Для CompleteFloor
    public bool noDamageCondition; // Для CompleteLevel

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerAchievement();
        }
    }

    // Можно вызывать вручную через другие скрипты
    public void TriggerAchievement()
    {
        switch (triggerType)
        {
            case AchievementTriggerType.StartGame:
                AchievementSystem.Instance.StartGame();
                break;

            case AchievementTriggerType.CompleteTutorial:
                AchievementSystem.Instance.CompleteTutorial();
                break;

            case AchievementTriggerType.CompleteLevel:
                AchievementSystem.Instance.CompleteLevel(noDamageCondition);
                break;

            case AchievementTriggerType.KillFinalBoss:
                AchievementSystem.Instance.KillFinalBoss();
                break;

            case AchievementTriggerType.CompleteFloor:
                AchievementSystem.Instance.CompleteFloor(floorNumber);
                break;

            case AchievementTriggerType.CompleteGame:
                AchievementSystem.Instance.CompleteGame();
                break;
        }

        // Опционально: отключаем триггер после срабатывания
        gameObject.SetActive(false);
    }
}

public enum AchievementTriggerType
{
    StartGame,
    CompleteTutorial,
    CompleteLevel,
    KillFinalBoss,
    CompleteFloor,
    CompleteGame
}