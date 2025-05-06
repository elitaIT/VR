using UnityEngine;
using UnityEngine.SceneManagement;  // Для работы с загрузкой сцен

public class SceneTeleportation : MonoBehaviour
{
    public string sceneToLoad;  // Имя сцены, в которую нужно телепортировать игрока

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггерную зону вошел игрок
        if (other.CompareTag("Player"))  // Убедитесь, что у вашего игрока есть тег "Player"
        {
            TeleportToScene();
        }
    }

    private void TeleportToScene()
    {
        // Загружаем сцену, имя которой указано в поле sceneToLoad
        SceneManager.LoadScene(sceneToLoad);
    }
}
