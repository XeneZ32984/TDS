using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthViewsController : MonoBehaviour
{
    // Константа минимальной позиции на экране
    private const float MinViewportPosition = -0.1f;

    // Константа максимальной позиции на экране
    private const float MaxViewportPosition = 1.1f;

    // Префаб для отображения шкалы здоровья врагов
    [SerializeField] private CharacterHealthView _enemyHealthViewPrefab;

    // Контейнер, в котором будет располагаться отображение шкалы здоровья
    [SerializeField] private Transform _enemyHealthViewsContainer;

    // Шкала здоровья зафиксирована над врагом
    // И будет перемещаться вместе с ним
    [SerializeField] private Vector3 _deltaHealthViewPosition = new Vector3(0, 2.2f, 0);

    // Словарь для связи шкал здоровья врагов и их отображений
    private Dictionary<CharacterHealth, CharacterHealthView> _enemyHealthViewPairs = new Dictionary<CharacterHealth, CharacterHealthView>();

    

    // Основная камера в игре
    private Camera _mainCamera;

    private void Start()
    {
        // Вызываем метод Init()
        Init();
    }

    private void Init()
    {
        // Присваиваем _mainCamera объект камеры
        _mainCamera = Camera.main;

        // Находим все объекты врагов с компонентом здоровья
        CharacterHealth[] enemyHealths = FindObjectsOfType<EnemyHealth>();

        // Проходим по массиву противников
        for (int i = 0; i < enemyHealths.Length; i++)
        {
            // Создаём отображение шкалы здоровья для каждого врага
            CreateEnemyHealthView(enemyHealths[i]);
        }
    }

    private void CreateEnemyHealthView(CharacterHealth health)
    {
        // Создаём экземпляр префаба отображения шкалы здоровья
        CharacterHealthView characterHealthView = Instantiate(_enemyHealthViewPrefab, _enemyHealthViewsContainer);

        // Устанавливаем позицию отображения шкалы здоровья на экране
        SetHealthViewScreenPosition(characterHealthView, health.transform.position);

        // Инициализируем отображение шкалы здоровья
        // Через ссылку на компонент здоровья врага
        characterHealthView.Init(health);

        // Добавляем пару «здоровье врага — отображение шкалы здоровья» в словарь
        _enemyHealthViewPairs.Add(health, characterHealthView);

        // Подписываемся на событие смерти врага
        // Чтобы удалить его отображение шкалы здоровья
        health.OnDieWithObject += DestroyEnemyHealthView;
    }

    private void DestroyEnemyHealthView(CharacterHealth health)
    {
        // Находим отображение шкалы здоровья врага
        CharacterHealthView view = _enemyHealthViewPairs[health];

        // Стираем пару «здоровье врага — отображение шкалы здоровья» из словаря
        _enemyHealthViewPairs.Remove(health);

        // Уничтожаем игровой объект отображения шкалы здоровья
        Destroy(view.gameObject);

        // Отписываемся от события смерти врага
        health.OnDieWithObject -= DestroyEnemyHealthView;
    }

    private void Update()
    {
        // Вызываем метод RefreshViewsPositions()
        RefreshViewsPositions();
    }

    private void RefreshViewsPositions()
    {
        // Проходим по всем парам в словаре
        foreach (var pair in _enemyHealthViewPairs)
        {
            // Получаем позицию врага с учётом смещения
            Vector3 enemyPosition = pair.Key.transform.position + _deltaHealthViewPosition;

            // Если позиция не видна на экране
            if (!CheckPositionVisible(enemyPosition))
            {
                // Пропускаем итерацию цикла
                continue;
            }
            // Устанавливаем позицию отображения шкалы здоровья
            SetHealthViewScreenPosition(pair.Value, enemyPosition);
        }
    }

    private bool CheckPositionVisible(Vector3 position)
    {
        // Преобразуем мировые координаты в экранные
        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(position);

        // Если экранные координаты позиции находятся за рамками видимости экрана
        if (viewportPosition.x < MinViewportPosition || viewportPosition.x > MaxViewportPosition
            || viewportPosition.y < MinViewportPosition || viewportPosition.y > MaxViewportPosition)
        {
            // Возвращаем false
            return false;
        }
        // Возвращаем true
        return true;
    }

    private void SetHealthViewScreenPosition(CharacterHealthView view,Vector3 worldPosition)
    {
        // Преобразуем мировые координаты в экранные
        view.transform.position = _mainCamera.WorldToScreenPoint(worldPosition);
    }
}
