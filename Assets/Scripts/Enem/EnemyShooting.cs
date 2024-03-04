using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : CharacterShooting
{
    // Префаб пули
    [SerializeField] private Bullet _bulletPrefab;

    // Задержка между выстрелами
    [SerializeField] private float _bulletDelay = 0.05f;

    // Дальность стрельбы
    [SerializeField] private float _shootingRange = 10f;

    // Количество пуль в обойме
    // То есть сколько пуль можно выстрелить подряд до перезарядки
    [SerializeField] private int _bulletsInRow = 7;

    // Время перезарядки
    [SerializeField] private float _reloadingDuration = 4f;

    // Точка появления пули
    private Transform _bulletSpawnPoint;

    // Позиция цели
    private Transform _targetTransform;

    // Счётчик времени между выстрелами
    private float _bulletTimer;

    // Текущее количество пуль в обойме
    private int _currentBulletsInRow;

    protected override void OnInit()
    {
        // Получаем компонент Transform для точки вылета пули
        _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;

        // Присваиваем _targetTransform значение
        // Любого объекта типа Player
        _targetTransform = FindAnyObjectByType<Player>().transform;

        // Перезагружаем оружие
        Reload();

        base.OnInit();

    }

    private void Reload()
    {
        // Обнуляем таймер выстрела
        _bulletTimer = 0;

        // Задаём текущее количество пуль в обойме
        _currentBulletsInRow = _bulletsInRow;
    }

    private void Update()
    {
        // Если враг не активен
        if (!IsActive)
        {
            // Выходим из метода
            return;
        }
        // Увеличиваем таймер выстрела
        // На время, прошедшее с последнего кадра
        _bulletTimer += Time.deltaTime;

        // Готовимся к стрельбе
        Shooting();

        // Перезаряжаем оружие
        Reloading();

        DamageBonusing();
    }

    private void Shooting()
    {
        // Если цель в пределах дальности стрельбы
        // И есть пули в обойме, и прошла задержка между выстрелами
        if (CheckTargetInRange() && CheckHasBulletsInRow() && _bulletTimer >= _bulletDelay)
        {
            // Стреляем по игроку
            Shoot();
        }
    }

    private void Reloading()
    {
        // Если пули в обойме закончились
        // И прошло время перезарядки
        if (!CheckHasBulletsInRow() && _bulletTimer >= _reloadingDuration)
        {
            // Перезагружаем оружие
            Reload();
        }
    }

    private bool CheckTargetInRange()
    {
        // Вычисляем расстояние между врагом и целью
        // Если оно <= дальности стрельбы, возвращаем true
        return (_targetTransform.position - transform.position).magnitude <= _shootingRange;
    }

    private bool CheckHasBulletsInRow()
    {
        // Вычисляем количество пуль в обойме
        // Если оно > 0, возвращаем true
        return _currentBulletsInRow > 0;
    }

    private void Shoot()
    {
        // Обнуляем таймер выстрела
        _bulletTimer = 0;

        // Делаем новую пулю
        SpawnBullet(_bulletPrefab, _bulletSpawnPoint);

        // Уменьшаем количество пуль в обойме
        _currentBulletsInRow--;
    }

    private void SpawnBullet()
    {
        // Создаём экземпляр префаба пули
        // В точке появления с теми же параметрами
        Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
    }
}
