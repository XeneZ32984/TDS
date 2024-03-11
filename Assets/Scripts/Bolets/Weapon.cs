using UnityEngine;

// Сделали класс абстрактным
public abstract class Weapon : MonoBehaviour
{
    // Урон от оружия
    [SerializeField] private int _damage = 10;

    // Публичное свойство Damage
    // Позволит получить приватное значение _damage
    public int Damage
    {
        // Специальная команда, чтобы получить значение
        get
        {
            // Возвращаем значение урона
            return _damage;
        }
    } 
    public void SetActive(bool value)
    {
        // Меняем активность объекта
        gameObject.SetActive(value);
    }
}