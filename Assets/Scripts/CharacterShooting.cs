using System;
using System.Collections.Generic;
using UnityEngine;

// Сделали класс абстрактным
public abstract class CharacterShooting : CharacterPart
{
   // Стандартный урон
    public const float DefaultDamageMutiplier = 1;

    // Объект оружия персонажа
    private Weapon _weapon;

    // Множитель урона
    private float _damageMultiplier = DefaultDamageMutiplier;

    // Длительность умноженного урона
    private float _damageMultiplierDuration;

    // Таймер умноженного урона
    private float _damageMultiplierTimer;

    // Событие установки множителя урона
    public Action<float> OnSetDamageMutiplier;

    // Событие изменения таймера урона
    public Action<float, float> OnChangeDamageTimer;

    // Свойство, чтобы узнать текущий множитель урона
    public float DamageMultiplier => _damageMultiplier;



    public void SetDamageMultiplier(float multiplier, float duration)
    {
        // Задаём множитель урона
        _damageMultiplier = multiplier;

        // Определяем длительность урона
        _damageMultiplierDuration = duration;

        // Обнуляем таймер урона
        _damageMultiplierTimer = 0;

        // Вызываем событие OnSetDamageMutiplier
        // Передаём в него множитель урона
        OnSetDamageMutiplier?.Invoke(_damageMultiplier);

        // Вызываем событие OnChangeDamageTimer
        // Передаём в него таймер и длительность урона
        OnChangeDamageTimer?.Invoke(_damageMultiplierTimer, _damageMultiplierDuration);
    }

    protected override void OnInit()
    {
        // Получаем Weapon из дочерних объектов
        _weapon = GetComponentInChildren<Weapon>();

        // Вызываем метод SetDefaultDamageMultiplier()
        SetDefaultDamageMultiplier();
    }

    protected void DamageBonusing()
    {
        // Если длительность урона <= 0
        if (_damageMultiplierDuration <= 0)
        {
            // Выходим из метода
            return;
        }
        // Увеличиваем таймер урона
        _damageMultiplierTimer += Time.deltaTime;

        // Вызываем событие OnChangeDamageTimer
        // Передаём в него таймер и длительность урона
        OnChangeDamageTimer?.Invoke(_damageMultiplierTimer, _damageMultiplierDuration);

        // Если значение таймера больше длительности
        if (_damageMultiplierTimer >= _damageMultiplierDuration)
        {
            // Вызываем метод SetDefaultDamageMultiplier()
            SetDefaultDamageMultiplier();
        }
    }

    protected void SpawnBullet(Bullet prefab, Transform spawnPoint)
    {
        // Создаём пулю в месте и направлении стрельбы
        Bullet bullet = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Инициализируем пулю (устанавливаем урон и т. д.)
        InitBullet(bullet);
    }

    private void SetDefaultDamageMultiplier()
    {
        // Устанавливаем стандартное увеличение урона
        SetDamageMultiplier(DefaultDamageMutiplier, 0);
    }

    private void InitBullet(Bullet bullet)
    {
        // Задаём урон от пули через урон оружия * множитель урона
        bullet.SetDamage((int) (_weapon.Damage * _damageMultiplier));
    }
}