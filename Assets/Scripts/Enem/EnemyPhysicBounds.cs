using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Задали наследование от CharacterPhysicBounds
public class EnemyPhysicBounds : CharacterPhysicBounds
{
    // Коллайдер врага
    private Collider _bodyCollider;

    // Инициализируем переменные
    protected override void OnInit()
    {
        // Присваиваем _bodyCollider компонент Collider из дочерних объектов
        _bodyCollider = GetComponentInChildren<Collider>();
    }
    // Останавливаем врага
    protected override void OnStop()
    {
        // Выключаем коллайдер
        _bodyCollider.enabled = false;
    }
}

