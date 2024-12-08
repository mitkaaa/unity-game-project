using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float baseMoveSpeed = 0.5f;        // Базовая минимальная скорость
    public float maxMoveSpeed = 4f;         // Максимальная скорость
    public float swipeScale = 100f;         // Коэффициент преобразования длины свайпа в скорость
    public float decelerationFactor = 0.95f;// Коэффициент замедления
    public float stopThreshold = 0.1f;      // Порог остановки скорости
    public float rotationSpeed = 10f;       // Скорость поворота
    private Vector2 touchStartPos;          // Начальная позиция тача
    private Vector2 touchCurrentPos;        // Текущая позиция тача
    private Vector2 swipeDirection;         // Направление свайпа
    private bool isMoving = false;          // Флаг движения
    private float currentSpeed;             // Текущая скорость

    // Animator animator;

    void Start()
    {
        // animator = GetComponent<Animator>();
        currentSpeed = baseMoveSpeed;  // Начальная скорость
    }

    void Update()
    {
        HandleTouchInput();
        MoveSphere();
        ApplyDeceleration();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;  // Запоминаем начальную точку
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchCurrentPos = touch.position;

                // Рассчитываем направление свайпа
                swipeDirection = (touchCurrentPos - touchStartPos).normalized;

                // Рассчитываем длину свайпа и увеличиваем скорость
                float swipeDistance = Vector2.Distance(touchStartPos, touchCurrentPos);
                currentSpeed = Mathf.Clamp(swipeDistance / swipeScale, baseMoveSpeed, maxMoveSpeed);

                // Устанавливаем флаг для начала движения
                isMoving = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isMoving = false;  // Останавливаем движение, если палец убран
            }
        }
    }

    private void MoveSphere()
    {
        if (isMoving)
        {
            // Получаем направление камеры в плоскости XZ
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;  // Игнорируем ось Y, чтобы не учитывать наклон камеры
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;  // Игнорируем ось Y, чтобы не учитывать наклон камеры
            cameraRight.Normalize();

            // Преобразуем свайп в направление движения относительно камеры
            Vector3 moveDirection = swipeDirection.x * cameraRight + swipeDirection.y * cameraForward;

            // Двигаем сферу в сторону свайпа с текущей скоростью
            transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

            // Поворачиваем сферу в направлении движения
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Обновляем анимацию скорости
            // animator.SetFloat("speed", currentSpeed);
        }
    }

    private void ApplyDeceleration()
    {
        if (!isMoving && currentSpeed > 0)
        {
            // Плавно замедляем скорость
            currentSpeed *= decelerationFactor;

            // Остановка, если скорость ниже порога
            if (currentSpeed < stopThreshold)
            {
                currentSpeed = 0f;
                // animator.SetFloat("speed", 0);  // Останавливаем анимацию
            }

            // Двигаем сферу на основе оставшейся инерции
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }
    }
}