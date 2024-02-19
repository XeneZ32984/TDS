using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealthView : MonoBehaviour
{
    // Трансформа шкалы процентов здоровья
    [SerializeField] private Transform _percentsImageTransform;

    // Очки здоровья персонажа
    private CharacterHealth _characterHealth;

    public void Init(CharacterHealth characterHealth)
    {
        // Задаём значение шкалы здоровья персонажа
        _characterHealth = characterHealth;

        // Подписываемся на событие OnAddHealthPoints
        // Указываем, что при возникновении события должен выполниться метод Refresh()
        _characterHealth.OnAddHealthPoints += Refresh;
    }

    private void Refresh()
    {
        // Вычисляем значение шкалы здоровья персонажа в процентах:
        // Делим его текущие очки здоровья на стартовые
        float percents = (float) _characterHealth.GetHealthPoints() / _characterHealth.GetStartHealthPoints();

        // Используем функцию Mathf.Clamp01()
        // Чтобы ограничить значение в диапазоне от 0 до 1
        percents = Mathf.Clamp01(percents);

        // Устанавливаем значение шкалы здоровья
        SetPercents(percents);
    }

    private void SetPercents(float value)
    {
        // Задаём масштаб трансформы объекта с процентами
        _percentsImageTransform.localScale = new Vector3(value, 1, 1);
    }

    private void OnDestroy()
    {
        // Отписываемся от события OnAddHealthPoints
        // При уничтожении персонажа
        _characterHealth.OnAddHealthPoints -= Refresh;
    }
}