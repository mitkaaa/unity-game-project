using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Вычисляем время между кадрами
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Настраиваем стиль текста
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(50, 50, 200, 50);  // Позиция и размер области вывода текста
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 30;  // Размер шрифта
        style.normal.textColor = Color.white;  // Цвет текста

        // Рассчитываем FPS
        float fps = 1.0f / deltaTime;
        string text = Mathf.Ceil(fps).ToString() + " FPS";

        // Выводим текст на экран
        GUI.Label(rect, text, style);
    }
}