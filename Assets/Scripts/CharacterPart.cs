using UnityEngine;

public abstract class CharacterPart : MonoBehaviour
{
    // Флаг активности персонажа
    protected bool IsActive;

    // Метод инициализации (больше не абстрактный)
    public virtual void Init()
    {
        // Делаем персонажа активным
        IsActive = true;

        // Вызываем метод OnInit()
        // Специфичный для дочерних объектов
        OnInit();
    }
    // Метод остановки персонажа
    public void Stop()
    {
        // Делаем персонажа неактивным
        IsActive = false;

        // Вызываем метод OnStop()
        // Специфичный для дочерних объектов
        OnStop();
    }

    // Виртуальный метод инициализации 
    protected virtual void OnInit() { }

    // Виртуальный метод остановки
    protected virtual void OnStop() { }
}