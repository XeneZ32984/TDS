using UnityEngine;

// Задаём наследование от IPhysicHittable
// Помимо стандартного от MonoBehaviour
public class PhysicObject : MonoBehaviour, IPhysicHittable
{
    // Компонент Rigidbody на физическом объекте
    private Rigidbody _rigidbody;

    // Реализуем метод Hit() из интерфейса
    public void Hit(Vector3 force, Vector3 position)
    {
        // Вызываем метод CheckRigidbody()
        CheckRigidbody();

        // Применяем к Rigidbody силу force в позиции position
        _rigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
    }
    // Проверяем наличие Rigidbody
    private void CheckRigidbody()
    {
        // Если переменная _rigidbody не задана
        if (!_rigidbody)
        {
            // Присваиваем ей Rigidbody текущего объекта
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}