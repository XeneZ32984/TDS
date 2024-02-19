using UnityEngine;

// Задали наследование от CharacterHealthView
public class PlayerHealthView : CharacterHealthView
{
    private void Start()
    {
        // Присваиваем playerHealth значение
        // Любого объекта типа PlayerHealth
        CharacterHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        // Инициализируем объект playerHealth
        Init(playerHealth);
    }
}