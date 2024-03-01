using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Минимальная позиция в пределах видимости экрана
    private const float MinViewportPosition = -0.1f;

    // Максимальная позиция в пределах видимости экрана 
    private const float MaxViewportPosition = 1.1f;

    // Префаб врага для генерации врагов
    [SerializeField] private Character _enemyPrefab;

    // Количество врагов, которых нужно создать
    [SerializeField] private int _enemyCount = 10;

    // Задержка между созданием врагов
    [SerializeField] private float _spawnDelay = 1f;

    // Массив точек, в которых могут появиться враги
    private EnemySpawnPoint[] _spawnPoints;

    // Основная камера игры
    private Camera _mainCamera;

    // Количество уже созданных врагов
    private int _spawnedEnemyCount;

    // Таймер для отслеживания задержки
    private float _spawnTimer;

    // Событие, которое вызывается при создании врага
    public Action<Character> OnSpawnEnemy;

    private void Start()
    {
        // Вызываем метод Init()
        Init();
    }

    private void Init()
    {
        // Заполняем массив _spawnPoints объектами
        // Типа EnemySpawnPoint, которые присутствуют на сцене
        _spawnPoints = FindObjectsOfType<EnemySpawnPoint>();

        // Присваиваем _mainCamera объект камеры
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // Генерируем новых врагов
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        // Если создали заданное количество врагов
        if (_spawnedEnemyCount >= _enemyCount)
        {
            // Выходим из метода
            return;
        }
        // Уменьшаем таймер создания врагов
        // На время, прошедшее с последнего кадра
        _spawnTimer -= Time.deltaTime;

        // Если таймер достиг нуля
        if (_spawnTimer <= 0)
        {
            // Создаём одного врага
            SpawnEnemy();

            // Сбрасываем значение таймера
            ResetSpawnTimer();
        }
    }

    private void SpawnEnemy()
    {
        // Получаем случайную точку появления врага
        EnemySpawnPoint spawnPoint = GetRandomSpawnPoint();

        // Создаём нового врага в указанной позиции
        Character newEnemy = Instantiate(_enemyPrefab, spawnPoint.transform.position, Quaternion.identity);

        // Увеличиваем количество уже созданных врагов
        _spawnedEnemyCount++;

        // Вызываем событие OnSpawnEnemy
        // Передаём в него созданного врага
        OnSpawnEnemy?.Invoke(newEnemy);
    }

    private EnemySpawnPoint GetRandomSpawnPoint()
    {
        // Получаем список возможных точек появления врагов
        // Которые находятся за пределами видимости камеры
        List<EnemySpawnPoint> possiblePoints = GetSpawnPointsOutOfCamera();

        // Если есть хотя бы одна возможная точка
        if (possiblePoints.Count > 0)
        {
            // Возвращаем случайную точку из списка возможных
            return possiblePoints[Random.Range(0, possiblePoints.Count)];
        }
        // Возвращаем случайную точку из массива всех точек
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }

    private List<EnemySpawnPoint> GetSpawnPointsOutOfCamera()
    {
        // Создаём новый список возможных точек появления врагов
        List<EnemySpawnPoint> possiblePoints = new List<EnemySpawnPoint>();

        // Заводим переменную для хранения позиции точки
        Vector3 pointViewportPosition;

        // Проходим по всем точкам
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            // Получаем позицию точки во Viewport-координатах
            pointViewportPosition = _mainCamera.WorldToViewportPoint(_spawnPoints[i].transform.position);

            // Если точка находится за пределами видимости экрана
            if (pointViewportPosition.x >= MinViewportPosition && pointViewportPosition.x <= MaxViewportPosition
                && pointViewportPosition.y >= MinViewportPosition && pointViewportPosition.y <= MaxViewportPosition)
            {
                // Пропускаем её и переходим к следующей точке
                continue;
            }
            // Добавляем её в список возможных точек
            possiblePoints.Add(_spawnPoints[i]);
        }
        // Возвращаем список возможных точек
        return possiblePoints;
    }

    private void ResetSpawnTimer()
    {
        // Приравниваем значение таймера к задержке
        _spawnTimer = _spawnDelay;
    }
}
