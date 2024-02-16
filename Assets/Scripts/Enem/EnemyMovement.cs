using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{

    // Константа с ключом горизонтального движения
    private const string MovementHorizontalKey = "Horizontal";

    // Константа с ключом вертикального движения
    private const string MovementVerticalKey = "Vertical";

    // Анимация противника
    private Animator _animator;

    // Навигационный агент
    // Управляет навигацией в игре
    private NavMeshAgent _navMeshAgent;

    // Трансформа главного героя
    // Враги будут бежать в его позицию
    private Transform _playerTransform;

    // Предыдущая позиция врага
    private Vector3 _prevPosition;

    protected override void OnInit()
    {
        // Присваиваем _animator компонент Animator из дочерних объектов
        _animator = GetComponentInChildren<Animator>();

        // Присваиваем _navMeshAgent компонент NavMeshAgent
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Присваиваем _playerTransform трансформу героя
        // FindAnyObjectByType<Player>() ищет игрока по типу Player
        _playerTransform = FindAnyObjectByType<Player>().transform;

        // Присваиваем _prevPosition текущую позицию врага
        _prevPosition = transform.position;
    }

    private void Update()
    {
        // НОВОЕ: Если враг не активен
        if (!IsActive)
        {
            // НОВОЕ: Выходим из метода
            return;
        }

        SetTargetPosition(_playerTransform.position);
        RefreshAnimation();;
    }

    private void SetTargetPosition(Vector3 position)
    {
        // Устанавливаем целевую позицию врага
        _navMeshAgent.SetDestination(position);
    }

    private void RefreshAnimation()
    {
        // Получаем текущую позицию врага
        Vector3 curPosition = transform.position;

        // Вычисляем разницу между текущей и предыдущей позицией
        Vector3 deltaMove = curPosition - _prevPosition;

        // Сохраняем текущую позицию в _prevPosition
        // Для использования при следующем обновлении анимации
        _prevPosition = curPosition;

        // Нормализуем разницу позиций, чтобы она имела длину 1
        deltaMove.Normalize();

        // Вычисляем относительное смещение по оси X
        float relatedX = Vector3.Dot(deltaMove, transform.right);

        // Вычисляем относительное смещение по оси Y
        float relatedY = Vector3.Dot(deltaMove, transform.forward);

        // Устанавливаем значения относительных смещений в аниматоре

        // Для смещения по горизонтали
        _animator.SetFloat(MovementHorizontalKey, relatedX);

        // Для смещения по вертикали
        _animator.SetFloat(MovementVerticalKey, relatedY);
    }

    protected override void OnStop()
    {
        // Отключаем навигационный агент
        _navMeshAgent.enabled = false;

        // Обновляем анимацию врага
        RefreshAnimation();
    }
}
