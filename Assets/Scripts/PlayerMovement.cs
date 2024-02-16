using UnityEngine;

// НОВОЕ: Добавили наследование от CharacterMovement
public class PlayerMovement : CharacterMovement
{
    private const string MovementHorizontalKey = "Horizontal";
    private const string MovementVerticalKey = "Vertical";

    private const string IsGroundedKey = "IsGrounded";

    [SerializeField] private float _gravityMultiplier = 2f;
    [SerializeField] private float _movementSpeed = 6f;

    [SerializeField] private float _jumpSpeed = 30f;
    [SerializeField] private float _jumpDuration = 1f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private float _groundCheckExtraUp = 0.2f;
    
    private Animator _animator;
    private CharacterController _characterController;

    private Camera _mainCamera;

    private Vector3 _groundCheckBox;

    private bool _isGrounded;
    private bool _isJumping;
    private float _jumpTimer;

    // НОВОЕ: Переопределили метод Init()
    protected override void OnInit()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        _groundCheckBox = new Vector3(_characterController.radius, 0.0001f, _characterController.radius);
    }

    private void FixedUpdate()
    {
        Gravity();

        // НОВОЕ: Если герой не активен
        if (!IsActive)
        {
            // НОВОЕ: Выходим из метода
            return;
        }

        Movement();
        Jumping();
    }

    private void Gravity()
    {
        Vector3 gravity = Physics.gravity;
        gravity *= _gravityMultiplier * Time.fixedDeltaTime;
        _characterController.Move(gravity);
    }

    private void Movement()
    {
        Vector3 movement = Vector3.zero;
        movement.x = Input.GetAxis(MovementHorizontalKey);
        movement.z = Input.GetAxis(MovementVerticalKey);

        movement = GetMovementByCamera(movement);
        movement *= _movementSpeed * Time.fixedDeltaTime;

        _characterController.Move(movement);
        AnimateMovement(movement);
    }

    private Vector3 GetMovementByCamera(Vector3 input)
    {
        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * input.z + cameraRight * input.x;
        return movement;
    }

    private void AnimateMovement(Vector3 movement)
    {
        float relatedX = Vector3.Dot(movement.normalized, transform.right);
        float relatedY = Vector3.Dot(movement.normalized, transform.forward);

        _animator.SetFloat(MovementHorizontalKey, relatedX);
        _animator.SetFloat(MovementVerticalKey, relatedY);

    }

    private void Jumping()
    {
        RefreshIsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isJumping)
        {
            SetIsGrounded(false);
            _isJumping = true;
            _jumpTimer = 0;
        }

        if (_isJumping)
        {
            _jumpTimer += Time.fixedDeltaTime;
            Vector3 motion = Vector3.up * _jumpSpeed * (1 - _jumpTimer / _jumpDuration) * Time.fixedDeltaTime;
            _characterController.Move(motion);
            if (_jumpTimer >= _jumpDuration || _isGrounded)
            {
                _isJumping = false;
            }
        }
    }

    private void RefreshIsGrounded()
    {
        SetIsGrounded(GroundCheck());
    }

    private bool GroundCheck()
    {
        Vector3 startCheckPosition = transform.position + Vector3.up * _groundCheckExtraUp;
        float checkDistance = _groundCheckDistance + _groundCheckExtraUp;
        return Physics.BoxCast(startCheckPosition, _groundCheckBox, Vector3.down, transform.rotation, checkDistance);
    }

    private void SetIsGrounded(bool value)
    {
        if (value != _isGrounded)
        {
            _animator.SetBool(IsGroundedKey, value);
        }
        _isGrounded = value;
    }

}