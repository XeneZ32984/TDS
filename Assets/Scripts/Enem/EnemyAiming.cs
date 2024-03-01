using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAiming : CharacterAiming
{

    // Скорость прицеливания врага
    [SerializeField] private float _aimingSpeed = 10f;

    // Вектор смещения прицела относительно позиции врага
    [SerializeField] private Vector3 _aimDeltaPosition = Vector3.up;

    // Дальность, на которой враг может прицелиться
    [SerializeField] private float _aimingRange = 20f;

    // Трансформа прицела врага
    private Transform _aimTransform;

    // Переменная для анимаций
    // Специального типа RigBuilder
    private RigBuilder _rigBuilder;

    // Массив объектов типа WeaponAiming
    private WeaponAiming[] _weaponAimings;

    // Трансформа цели врага
    private Transform _targetTransform;

    // Флаг доступности цели
    private bool _isTargetInRange;

    protected override void OnInit()
    {
        // Создаём целевой объект и получаем его трансформу
        _aimTransform = CreateAim().transform;

        // Получаем RigBuilder из дочерних объектов
        // Записываем его в переменную _rigBuilder
        _rigBuilder = GetComponentInChildren<RigBuilder>();

        // Получаем все компоненты WeaponAiming
        // Записываем их в массив _weaponAimings
        _weaponAimings = GetComponentsInChildren<WeaponAiming>(true);

        // Присваиваем _targetTransform значение
        // Любого объекта типа Player
        _targetTransform = FindAnyObjectByType<Player>().transform;

        // Вызываем метод SetDefaultAimPosition()
        SetDefaultAimPosition();

        // Вызываем метод InitWeaponAimings()
        // Передаём туда _weaponAimings и _aimTransform
        InitWeaponAimings(_weaponAimings, _aimTransform);
    }

    private GameObject CreateAim()
    {
        // Создаём пустой объект с именем EnemyAim
        GameObject aim = new GameObject("EnemyAim");

        // Делаем aim дочерним объектом врага
        aim.transform.SetParent(transform);

        // Возвращаем объект aim
        return aim;
    }

    private void SetDefaultAimPosition()
    {
        // Задаём позицию цели врага
        // Через направление взгляда и вектор смещения
        _aimTransform.position = transform.position + transform.forward + _aimDeltaPosition;
    }

    private void InitWeaponAimings(WeaponAiming[] weaponAimings, Transform aim)
    {
        // Проходим по всем элементам weaponAimings
        for (int i = 0; i < weaponAimings.Length; i++)
        {
            // Вызываем у weaponAimings[i] метод Init()
            // И передаём ему компонент цели aim
            weaponAimings[i].Init(aim);
        }
        // Вызываем у _rigBuilder встроенный метод Build()
        // Чтобы построить скелетную анимацию врага
        _rigBuilder.Build();
    }

    private void Update()
    {
        // Если враг не активен
        if (!IsActive)
        {
            // Выходим из метода
            return;
        }
        // Поворачиваем врага для прицеливания
        Aiming();
    }

    private void Aiming()
    {
        // Если цель в пределах доступности
        if (CheckTargetInRange())
        {
            // Ставим флаг «Цель доступна»
            _isTargetInRange = true;

            // Плавно перемещаем цель врага
            // В точку столкновения с заданной скоростью
            _aimTransform.position = Vector3.Lerp(_aimTransform.position, _targetTransform.position + _aimDeltaPosition, _aimingSpeed * Time.deltaTime);
        }
        // Иначе
        else
        {
            // Если цель сейчас не доступна
            // Но была доступна в предыдущем кадре
            if (_isTargetInRange)
            {
                // Ставим флаг «Цель недоступна»
                _isTargetInRange = false;

                // Устанавливаем позицию цели по умолчанию
                SetDefaultAimPosition();
            }
        }
        // Вычисляем направление взгляда врага
        // Чтобы смотреть на точку пересечения луча с объектом
        Vector3 lookDirection = (_aimTransform.position - transform.position).normalized;

        // Обнуляем вертикальную составляющую направления
        // Чтобы враг не наклонялся вверх или вниз 
        lookDirection.y = 0;

        // Создаём новый поворот врага
        // Так, чтобы он смотрел в заданном направлении
        var newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Плавно поворачиваем врага с учётом скорости
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _aimingSpeed * Time.deltaTime);
    }

    private bool CheckTargetInRange()
    {
        // Вычисляем расстояние между врагом и целью
        // Если оно <= дальности прицеливания, возвращаем true
        return (_targetTransform.position - transform.position).magnitude <= _aimingRange;
    }
}
