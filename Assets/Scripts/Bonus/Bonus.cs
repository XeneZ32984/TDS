using UnityEngine;

// Сделали класс абстрактным
public abstract class Bonus : MonoBehaviour
{
    // Абстрактный метод проверки столкновений
    protected abstract bool CheckTriggeredObject(Collider other);

    // Абстрактный метод применения бонуса
    protected abstract void ApplyBonus();

    // Проверяем касание бонуса
    private void OnTriggerEnter(Collider other)
    {
        // Если бонуса коснулся нужный объект
        // Например, игрок
        if (CheckTriggeredObject(other))
        {
            // Применяем бонус
            ApplyBonus();

            // Уничтожаем бонус
            DestroyBonus();
        }
    }
    // Убираем бонус из игры
    private void DestroyBonus()
    {
        // Удаляем объект бонуса
        Destroy(gameObject);
    }
}