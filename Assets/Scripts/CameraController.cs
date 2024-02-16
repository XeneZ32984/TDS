using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Трансформа цели — игрок
    [SerializeField] private Transform _target;

    // Трансформа объекта, который содержит камеру
    [SerializeField] private Transform _cameraRoot;

    // Трансформа самой камеры
    [SerializeField] private Transform _cameraTransform;

    // Скорость перемещения камеры
    [SerializeField] private float _moveSpeed = 5f;

    // Отступ позиции камеры от цели
    [SerializeField] private Vector3 _positionOffset = Vector3.up;

    // Скорость вращения камеры
    [SerializeField] private float _rotationSpeed = 65f;

    // Скорость изменения зума камеры
    [SerializeField] private float _zoomSpeed = 10f;

    // Множитель изменения зума камеры с помощью мыши
    [SerializeField] private float _mouseZoomMultiplier = 3f;

    // Минимальное значение зума камеры
    [SerializeField] private float _minZoom = 3f;

    // Максимальное значение зума камеры
    [SerializeField] private float _maxZoom = 14f;

    // Текущее значение зума камеры
    private float _currentZoom;


    private void Start()
    {
        // Вызываем метод Init()
        Init();
    }

    private void Init()
    {
        // Вычисляем текущий зум камеры
        // Через расстояние между целью и трансформой камеры
        _currentZoom = (_target.position - _cameraTransform.position).magnitude;
    }

    private void LateUpdate()
    {
        // Перемещаем камеру
        MoveCamera();

        // Вращаем камеру
        RotateCamera();

        // Управляем зумом
        ZoomCamera();
    }

    private void MoveCamera()
    {
        // Если цель или корневой объект камеры не заданы
        if (!_target || !_cameraRoot)
        {
            // Выходим из метода
            return;
        }
        // Рассчитываем позицию цели с учётом отступа
        Vector3 targetPosition = _target.position + _positionOffset;

        // Плавно перемещаем корневой объект камеры к цели
        _cameraRoot.transform.position = Vector3.Lerp(_cameraRoot.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        // Если корневого объекта камеры нет
        if (!_cameraRoot)
        {
            // Выходим из метода
            return;
        }
        // Заводим переменную для направления поворота
        float direction = 0;

        // Если нажата клавиша Q
        if (Input.GetKey(KeyCode.Q))
        {
            // Камера будет поворачиваться по часовой стрелке
            direction = 1;
        }
        // Иначе, если нажата клавиша E
        else if (Input.GetKey(KeyCode.E)) 
        {
            // Камера будет поворачиваться против часовой стрелки
            direction = -1;
        }
        // Если направление равно 0
        // Не нажата ни клавиша Q, ни клавиша E
        if (Mathf.Approximately(direction, 0))
        {
            // Выходим из метода
            return;
        }
        // Получаем углы корневого объекта камеры
        Vector3 cameraEuler = _cameraRoot.eulerAngles;

        // Изменяем угол поворота камеры
        // На произведение направления, скорости и времени
        cameraEuler.y += direction * _rotationSpeed * Time.deltaTime;

        // Присваиваем новые углы корневому объекту камеры
        _cameraRoot.eulerAngles = cameraEuler;
    }

    private void ZoomCamera()
    {
        // Если трансформы главной камеры нет
        if (!_cameraTransform)
        {
            // Выходим из метода
            return;
        }
        // Заводим переменную для направления поворота
        float direction = 0;

        // Если нажата клавиша Z или клавиша минус
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.KeypadMinus))
        {
            // Камера будет приближаться к карте
            direction = 1;
        }
        // Иначе, если нажата клавиша X или клавиша плюс
        else if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.KeypadPlus))
        {
            // Камера будет отдаляться от карты
            direction = -1;
        }
        // Иначе, если прокручено колесо мыши или тачпад
        else if (!Mathf.Approximately(Input.mouseScrollDelta.y, 0))
        {
            // Направление зума будет зависеть от этого значения
            direction = - Input.mouseScrollDelta.y * _mouseZoomMultiplier;
        }
        // Если направление равно 0
        // То есть не было действий, описанных ранее
        if (Mathf.Approximately(direction, 0))
        {
            // Выходим из метода
            return;
        }
        // Изменяем текущий зум
        // На произведение направления, скорости и времени
        _currentZoom += direction * _zoomSpeed * Time.deltaTime;

        // Ограничиваем текущий зум
        // В пределах минимального и максимального
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

        // Изменяем позицию трансформы главной камеры
        // Чтобы она была на расстоянии текущего зума от корня
        _cameraTransform.position = _cameraRoot.position - _cameraTransform.forward * _currentZoom;
    }
}
