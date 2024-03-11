using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : CharacterShooting
{
    // Префаб пули
    [SerializeField] private Bullet _bulletPrefab;

    // Задержка между выстрелами
    [SerializeField] private float _bulletDelay = 0.05f;

    // Точка появления пули
    private Transform _bulletSpawnPoint;

    // Счётчик времени между выстрелами
    private float _bulletTimer;

    protected override void OnInit()
    {
        // Получаем компонент Transform для точки вылета пули
        _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform;
        
        // Обнуляем таймер выстрела
        _bulletTimer = 0;

        base.OnInit();
    }

    private void Update()
    {
        // НОВОЕ: Если герой не активен
        if (!IsActive)
        {
            // НОВОЕ: Выходим из метода
            return;
        }

        Shooting();
        DamageBonusing();
    }

    private void Shooting()
    {
        // Если нажата левая кнопка мыши
        if (Input.GetMouseButton(0))
        {
            // Увеличиваем таймер выстрела
            // На время, прошедшее с предыдущего кадра
            _bulletTimer += Time.deltaTime;

            // Если достигнуто значение задержки
            if (_bulletTimer >= _bulletDelay)
            {
                // Обнуляем таймер выстрела
                _bulletTimer = 0;

                // Делаем новую пулю
                SpawnBullet(_bulletPrefab, _bulletSpawnPoint);
            }
        }
    }
}
