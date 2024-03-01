using UnityEngine;

// Сделали наследование от Bonus
public class HealthBonus : Bonus
{
    // Количество здоровья, которое добавится игроку
    [SerializeField] private int _health = 50;

    // Объект здоровья игрока
    private CharacterHealth _healingCharacterHealth;

    // Переопределили метод CheckTriggeredObject()
    protected override bool CheckTriggeredObject(Collider other)
    {
        // Ищем на объекте, с которым столкнулись, компонент PlayerHealth
        // То есть проверяем, что бонус получает игрок
        _healingCharacterHealth = other.GetComponentInParent<PlayerHealth>();

        // Возвращаем true, если нашли PlayerHealth
        // Иначе возвращаем false
        return _healingCharacterHealth != null;
    }
    // Переопределили метод ApplyBonus()
    protected override void ApplyBonus()
    {
        // Добавляем игроку очки здоровья
        _healingCharacterHealth.AddHealthPoints(_health);
    }
}