using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  
    public Vector3 fixedOffset = new Vector3(0, 6, -4);  // Смещение по высоте и отдаление

    void LateUpdate()
    {
        // Камера всегда остаётся на позиции игрока с учётом смещения
        transform.position = player.position + fixedOffset;
    }
}