using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class GameAchievement : MonoBehaviour
{
    public string id;
    public string title;
    [TextArea] public string description;
    public Sprite icon;
    public bool isUnlocked;
    public bool isHidden;
}

[System.Serializable]
public class AchievementUnlockedEvent : UnityEvent<GameAchievement> { }

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance;

    [Header("Настройки")]
    public bool saveProgress = true;
    public string saveKey = "AchievementsData";

    [Header("Достижения")]
    public List<GameAchievement> achievements = new List<GameAchievement>();

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

        // Найти все компоненты GameAchievement в сцене, если список пуст
        if (achievements.Count == 0)
        {
            GameAchievement[] foundAchievements = FindObjectsOfType<GameAchievement>();
            achievements.AddRange(foundAchievements);
        }

        LoadAchievements();
        initialized = true;
    }

    public void StartGame()
    {
        UnlockAchievement("Новичок");
    }

    public void CompleteTutorial()
    {
        UnlockAchievement("Обучен - значит вооружен!");
    }

    public void TakeDamage()
    {
        tookDamage = true;
    }

    public void CompleteLevel(bool noDamage)
    {
        if (noDamage && !tookDamage)
        {
            UnlockAchievement("Скупой на здоровье");
        }
        tookDamage = false;
    }

    public void KillFinalBoss()
    {
        UnlockAchievement("Финальный босс побежден!");
    }

    public void CompleteFloor(int floorNumber)
    {
        switch (floorNumber)
        {
            case 1: UnlockAchievement("Пройди 1 этаж"); break;
            case 2: UnlockAchievement("Пройди 2 этаж"); break;
            case 3: UnlockAchievement("Пройди 3 этаж"); break;
            case 4:
                UnlockAchievement("Пройди 4 этаж");
                if (!tookDamage)
                {
                    UnlockAchievement("no_damage_run");
                }
                break;
        }
    }

    public void CompleteGame()
    {
        UnlockAchievement("Пройди игру");
    }

    public void UnlockAchievement(string id)
    {
        GameAchievement achievement = achievements.Find(a => a.id == id);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log($"Достижение получено: {achievement.title} - {achievement.description}");

            onAchievementUnlocked.Invoke(achievement);

            if (saveProgress)
            {
                SaveAchievements();
            }
        }
    }

    public bool IsAchievementUnlocked(string id)
    {
        GameAchievement achievement = achievements.Find(a => a.id == id);
        return achievement?.isUnlocked ?? false;
    }

    public List<GameAchievement> GetUnlockedAchievements()
    {
        return achievements.FindAll(a => a.isUnlocked);
    }

    public List<GameAchievement> GetLockedAchievements(bool includeHidden = false)
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
            GameAchievement achievement = achievements.Find(a => a.id == id);
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
