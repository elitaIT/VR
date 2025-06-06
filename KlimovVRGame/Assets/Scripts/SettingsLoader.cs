using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsLoader : MonoBehaviour
{
    private static SettingsLoader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // уже есть, удаляем дубликат
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        MoveSettings move = FindObjectOfType<MoveSettings>();
        if (move)
        {
            Debug.Log("[SettingsLoader] Применение MoveSettings");
            move.ApplySavedMovementMode();
        }

        RotateSettings rotate = FindObjectOfType<RotateSettings>();
        if (rotate)
        {
            Debug.Log("[SettingsLoader] Применение RotateSettings");
            rotate.ApplySavedTurnMode();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
