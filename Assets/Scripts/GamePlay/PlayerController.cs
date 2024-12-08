using UnityEngine;

public class PlayerControllerTouch : MonoBehaviour
{
    [Header("Характеристики игрока")]
    public float walkSpeed = 3f;    // Скорость ходьбы
    public float runSpeed = 6f;     // Скорость бега
    public float rotationSpeed = 720f; // Скорость поворота

    private Animator animator;
    private Rigidbody rb;

    private Vector2 initialTouchPosition; // Начальная позиция тача
    private float touchDistanceThreshold = 100f; // Порог для перехода в бег (в пикселях)

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchMovement();
        }
        else
        {
            StopMovement();
        }
    }

    void HandleTouchMovement()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Сохраняем начальную позицию касания
            initialTouchPosition = touch.position;
        }

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            // Рассчитываем смещение от начальной точки касания
            Vector2 touchDelta = touch.position - initialTouchPosition;

            // Определяем скорость: Walk или Run
            float touchDistance = touchDelta.magnitude;
            float currentSpeed = touchDistance > touchDistanceThreshold ? runSpeed : walkSpeed;

            // Определяем направление движения на экране
            Vector3 screenDirection = new Vector3(touchDelta.x, 0, touchDelta.y).normalized;

            // Преобразуем экранное направление в мировое направление
            Vector3 worldDirection = Camera.main.transform.TransformDirection(screenDirection);
            worldDirection.y = 0; // Убираем вертикальную составляющую

            // Движение
            Vector3 move = worldDirection * currentSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + move);

            // Поворот в направлении движения
            if (worldDirection.magnitude > 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(worldDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            // Обновление анимации
            UpdateAnimations(currentSpeed);
        }
    }

    void StopMovement()
    {
        UpdateAnimations(0);
    }

    void UpdateAnimations(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);

            // Устанавливаем состояние Walk или Run в зависимости от скорости
            bool isRunning = speed >= runSpeed * 0.8f; // Если скорость ближе к бегу
            animator.SetBool("IsRunning", isRunning);
        }
    }
}
