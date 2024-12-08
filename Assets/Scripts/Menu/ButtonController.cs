using UnityEngine;
using UnityEngine.SceneManagement;  // Импорт для работы с загрузкой сцен

public class ButtonController : MonoBehaviour
{
    // Этот метод будет вызываться по клику на кнопку
    public void LoadNewScene(string sceneName)
    {
        // Загружаем сцену с именем, которое передается в параметр
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}