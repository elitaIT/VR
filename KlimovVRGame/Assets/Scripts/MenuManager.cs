using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public XRUIInputModule vrInputModule;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        EnsureUIInputActive();
    }

    public void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        EnsureUIInputActive();
    }

    private void EnsureUIInputActive()
    {
        // Гарантируем, что ввод работает после переключения
        if (vrInputModule != null)
        {
            vrInputModule.enabled = false;
            vrInputModule.enabled = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}