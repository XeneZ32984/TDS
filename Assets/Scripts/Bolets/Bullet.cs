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

    public int damage = 10;

    private int _damage;

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
    {     // Обрабатываем попадание
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _speed * Time.deltaTime) && !hit.collider.isTrigger)
            {
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

        CheckPhysicObjectHit(hit);

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
        CharacterHealth hittedHealth = hit.collider.GetComponentInChildren<CharacterHealth>();

        if (hittedHealth)
        {
            // НОВОЕ: Убрали здесь int damage = 10;

            // НОВОЕ: Заменили damage на _damage
            hittedHealth.AddHealthPoints(-_damage);
        }
    }

    private void CheckPhysicObjectHit(RaycastHit hit)
    {
        // Создаём переменную для объекта воздействия
        IPhysicHittable hittedPhysicObject = hit.collider.GetComponentInParent<IPhysicHittable>();

        // Если объект не пустой (существует)
        if (hittedPhysicObject != null)
        {
            // Вызываем у него метод попадания Hit()
            // Передаём направление пули, умноженное на её скорость
            // А также точку, в которую попала пуля на объекте
            hittedPhysicObject.Hit(transform.forward * _speed * 5, hit.point);
        }
    }
    public void SetDamage(int value)
    {
        // Делаем урон равным value
        _damage = value;
    }
}
