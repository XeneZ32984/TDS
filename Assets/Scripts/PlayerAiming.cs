using UnityEngine;
using UnityEngine.Animations.Rigging;

// НОВОЕ: Добавили наследование от CharacterAiming
public class PlayerAiming : CharacterAiming
{
    [SerializeField] private float _aimingSpeed = 10f;

    private Transform _aimTransform;
    private RigBuilder _rigBuilder;
    private WeaponAiming[] _weaponAimings;

    private Camera _mainCamera;

    // НОВОЕ: Переопределили метод Init()
    protected override void OnInit()
    {
        _mainCamera = Camera.main;
        _aimTransform = FindAnyObjectByType<PlayerAim>().transform;
        _rigBuilder = GetComponentInChildren<RigBuilder>();
        _weaponAimings = GetComponentsInChildren<WeaponAiming>(true);

        InitWeaponAimings(_weaponAimings, _aimTransform);
    }

    private void InitWeaponAimings(WeaponAiming[] weaponAimings, Transform aim)
    {
        for (int i = 0; i < weaponAimings.Length; i++)
        {
            weaponAimings[i].Init(aim);
        }
        _rigBuilder.Build();
    }

    private void FixedUpdate()
    {
        // НОВОЕ: Если герой не активен
        if (!IsActive)
        {
            // НОВОЕ: Выходим из метода
            return;
        }
        Aiming();
    }

    private void Aiming()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray findTargetRay = _mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(findTargetRay, out RaycastHit hitInfo))
        {
            Vector3 lookDirection = (hitInfo.point - transform.position).normalized;
            lookDirection.y = 0;
            var newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _aimingSpeed * Time.fixedDeltaTime);

            _aimTransform.position = Vector3.Lerp(_aimTransform.position, hitInfo.point, _aimingSpeed * Time.fixedDeltaTime);
        }
    }
}