using System;

using UnityEngine;

public abstract class CharacterHealth : CharacterPart
{

    // Константа с ключом смерти персонажа
    private const string DeathKey = "Death";

    // Стартовое количество здоровья
    [SerializeField] private int _startHealthPoints = 100;

    // Аниматор персонажа
    private Animator _animator;

    // Очки здоровья персонажа
    private int _healthPoints;

    // Флаг смерти персонажа
    private bool _isDead;

    // Событие при смерти
    public Action OnDie;

    // Событие при гибели персонажа
    // Со ссылкой на метод с параметром типа CharacterHealth
    public Action<CharacterHealth> OnDieWithObject;

    // Событие при изменении очков здоровья
    public Action OnAddHealthPoints;

    public void AddHealthPoints(int value)
    {
        // Если персонаж мёртв
        if (_isDead)
        {
            // Выходим из метода
            return;
        }
        // Увеличиваем значение здоровья на value
        _healthPoints += value;

        // НОВОЕ: Проверяем, что здоровье в пределах от нуля до заданного изначально
        _healthPoints = Mathf.Clamp(_healthPoints, 0, _startHealthPoints);

        // НОВОЕ: Вызываем событие OnAddHealthPoints
         OnAddHealthPoints?.Invoke();

        // Если здоровье достигло нуля
        if (_healthPoints <= 0)
        {
            // Обрабатываем смерть персонажа
            Die();
        }
    }

    protected override void OnInit()
    {
        // Присваиваем _animator компонент Animator из дочерних объектов
        _animator = GetComponentInChildren<Animator>();

        // Задаём начальное значение здоровья
        _healthPoints = _startHealthPoints;

        // Ставим флаг в значение «живой»
        _isDead = false;
    }

    private void Die()
    {
        // Ставим флаг в значение «мёртвый»
        _isDead = true;

        // Запускаем анимацию смерти персонажа
        _animator.SetTrigger(DeathKey);

        // Вызываем событие OnDie
        OnDie?.Invoke();

        OnDieWithObject?.Invoke(this);
    }

    public int GetStartHealthPoints()
    {
        // Возвращаем стартовое значение здоровья
        return _startHealthPoints;
    }

    public int GetHealthPoints()
    {
        // Возвращаем текущее значение здоровья
        return _healthPoints;
    }
}
