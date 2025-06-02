using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Переносим класс Achievement на уровень namespace
[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    [TextArea] public string description;
    public Sprite icon;
    public bool isUnlocked;
    public bool isHidden; // Скрыто ли достижение до разблокировки
}

[System.Serializable]
public class AchievementUnlockedEvent : UnityEvent<Achievement> { }

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance;

    [Header("Настройки")]
    public bool saveProgress = true;
    public string saveKey = "AchievementsData";

    [Header("Достижения")]
    public List<Achievement> achievements = new List<Achievement>()
    {
        new Achievement(){id = "start_game", title = "Новичок", description = "Запустите игру впервые", isUnlocked = false},
        new Achievement(){id = "complete_tutorial", title = "Ученик", description = "Пройдите обучение", isUnlocked = false},
        new Achievement(){id = "no_damage_level", title = "Неуязвимый", description = "Пройдите уровень без урона", isUnlocked = false, isHidden = true},
        new Achievement(){id = "kill_final_boss", title = "Победитель босса", description = "Победите финального босса", isUnlocked = false},
        new Achievement(){id = "floor_1", title = "Первый этаж пройден", description = "Пройдите первый этаж", isUnlocked = false},
        new Achievement(){id = "floor_2", title = "Второй этаж пройден", description = "Пройдите второй этаж", isUnlocked = false},
        new Achievement(){id = "floor_3", title = "Третий этаж пройден", description = "Пройдите третий этаж", isUnlocked = false},
        new Achievement(){id = "floor_4", title = "Четвертый этаж пройден", description = "Пройдите четвертый этаж", isUnlocked = false},
        new Achievement(){id = "no_damage_run", title = "Совершенство", description = "Пройдите игру без получения урона", isUnlocked = false, isHidden = true},
        new Achievement(){id = "complete_game", title = "Мастер игры", description = "Полностью завершите игру", isUnlocked = false}
    };

    [Header("События")]
    public AchievementUnlockedEvent onAchievementUnlocked;

    private bool tookDamage = false;
    private bool initialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        if (initialized) return;

        LoadAchievements();
        initialized = true;
    }

    public void StartGame()
    {
        UnlockAchievement("start_game");
    }

    public void CompleteTutorial()
    {
        UnlockAchievement("complete_tutorial");
    }

    public void TakeDamage()
    {
        tookDamage = true;
    }

    public void CompleteLevel(bool noDamage)
    {
        if (noDamage && !tookDamage)
        {
            UnlockAchievement("no_damage_level");
        }
        tookDamage = false;
    }

    public void KillFinalBoss()
    {
        UnlockAchievement("kill_final_boss");
    }

    public void CompleteFloor(int floorNumber)
    {
        switch (floorNumber)
        {
            case 1: UnlockAchievement("floor_1"); break;
            case 2: UnlockAchievement("floor_2"); break;
            case 3: UnlockAchievement("floor_3"); break;
            case 4:
                UnlockAchievement("floor_4");
                if (!tookDamage)
                {
                    UnlockAchievement("no_damage_run");
                }
                break;
        }
    }

    public void CompleteGame()
    {
        UnlockAchievement("complete_game");
    }

    public void UnlockAchievement(string id)
    {
        Achievement achievement = achievements.Find(a => a.id == id);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true; Debug.Log($"Достижение получено: {achievement.title} - {achievement.description}");

            onAchievementUnlocked.Invoke(achievement);

            if (saveProgress)
            {
                SaveAchievements();
            }
        }
    }

    public bool IsAchievementUnlocked(string id)
    {
        Achievement achievement = achievements.Find(a => a.id == id);
        return achievement?.isUnlocked ?? false;
    }

    public List<Achievement> GetUnlockedAchievements()
    {
        return achievements.FindAll(a => a.isUnlocked);
    }

    public List<Achievement> GetLockedAchievements(bool includeHidden = false)
    {
        return achievements.FindAll(a => !a.isUnlocked && (!a.isHidden || includeHidden));
    }

    private void SaveAchievements()
    {
        AchievementSaveData saveData = new AchievementSaveData();
        foreach (var achievement in achievements)
        {
            if (achievement.isUnlocked)
            {
                saveData.unlockedAchievementIds.Add(achievement.id);
            }
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        if (!PlayerPrefs.HasKey(saveKey)) return;

        string json = PlayerPrefs.GetString(saveKey);
        AchievementSaveData saveData = JsonUtility.FromJson<AchievementSaveData>(json);

        foreach (var id in saveData.unlockedAchievementIds)
        {
            Achievement achievement = achievements.Find(a => a.id == id);
            if (achievement != null)
            {
                achievement.isUnlocked = true;
            }
        }
    }

    [System.Serializable]
    private class AchievementSaveData
    {
        public List<string> unlockedAchievementIds = new List<string>();
    }
}