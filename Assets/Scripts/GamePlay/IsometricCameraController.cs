using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    // Объект, за которым будет следить камера
    public Transform target; 

    // Смещение камеры относительно цели
    public Vector3 offset = new Vector3(0, 10, -10);

    // Скорость следования камеры за целью
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Цель для камеры не установлена!");
            return;
        }

        // Позиция, куда камера должна переместиться
        Vector3 targetPosition = target.position + offset;

        targetPosition.y = offset.y;

        // Плавное перемещение камеры к цели с учетом возможного столкновения
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Смотрим на цель
        transform.LookAt(target);
    }
}
