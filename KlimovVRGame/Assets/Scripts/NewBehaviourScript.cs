using UnityEngine;
using UnityEngine.SceneManagement;  // ��� ������ � ��������� ����

public class SceneTeleportation : MonoBehaviour
{
    public string sceneToLoad;  // ��� �����, � ������� ����� ��������������� ������

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� � ���������� ���� ����� �����
        if (other.CompareTag("Player"))  // ���������, ��� � ������ ������ ���� ��� "Player"
        {
            TeleportToScene();
        }
    }

    private void TeleportToScene()
    {
        // ��������� �����, ��� ������� ������� � ���� sceneToLoad
        SceneManager.LoadScene(sceneToLoad);
    }
}
