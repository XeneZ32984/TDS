using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Префаб, который будет появляться при попадании пули
    [SerializeField] private GameObject _hitPrefab;

    // Скорость пули
    [SerializeField] private float _speed = 30f;

    // Время отображения пули на экране
    [SerializeField] private float _lifeTime = 2f;

    private void Update()
    {
        // Уменьшаем время отображения пули на экране
        ReduceLifeTime();

        // Проверяем попадание в объект
        CheckHit();

        // Перемещаем пулю
        Move();
    }

    private void ReduceLifeTime()
    {
        // Сокращаем время отображения пули на время, прошедшее с последнего кадра
        _lifeTime -= Time.deltaTime;

        // Если время отображения пули истекло
        if (_lifeTime <= 0)
        {
            // Пуля пропадает с экрана
            DestroyBullet();
        }
    }

    private void CheckHit()
    {
        // Если пуля столкнулась с чем-то
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _speed * Time.deltaTime))
        {
            // Обрабатываем попадание
            Hit(hit);
        }
    }

    private void Move()
    {
        // Меняем позицию пули через изменения скорости и времени
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Hit(RaycastHit hit)
    {
        // Создаём эффект попадания на месте столкновения пули
        Instantiate(_hitPrefab, hit.point, Quaternion.LookRotation(-transform.up, -transform.forward));

        CheckCharacterHit(hit);

        // Пуля пропадает с экрана
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        // Убираем объект пули
        Destroy(gameObject);
    }

    private void CheckCharacterHit(RaycastHit hit)
    {
        // Получаем компонент CharacterHealth
        // У персонажа, в которого попала пуля
        CharacterHealth hittedHealth = hit.collider.GetComponentInChildren<CharacterHealth>();

        // Если такой компонент есть
        // То есть пуля попала в персонажа
        if (hittedHealth)
        {
            // Задаём урон от пули; временно поставим его здесь
            // Позже будем задавать урон извне этого метода
            int damage = 10;

            // Уменьшаем количество здоровья персонажа
            hittedHealth.AddHealthPoints(-damage);
        }
    }
}
