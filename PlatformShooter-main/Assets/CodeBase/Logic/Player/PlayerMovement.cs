using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;

    [Header("Main parameters:")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _airControl;

    [Header("Additional parameters:")]
    [SerializeField] private float _maxAcceleration;
    [SerializeField] private float _maxVelocityMagnitude;
    [SerializeField] private float _maxDropAnimationAcceleration;
    [SerializeField] private float _angleControl;
    [SerializeField] private float _groundAcceleration;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _controlMovement;
    [SerializeField] private float _airControlMovement;
    [SerializeField] private float _controlDrop;

    [Header("Debug purposes:")]
    [SerializeField] private Vector3 _accelerationVector;
    [SerializeField] private Vector3 _playerVelocity;
    [SerializeField] private float _acceleration;
    public bool IsGrounded;

    private PlayerAnimations _playerAnimations;

    private Vector3 _animationVelocity;
    private Vector3 _wishedDirection = Vector3.zero;
    private bool _wishedJump;
    private readonly InputService _inputService = InputService.Instance;
    private const float Gravity = -9.81f;

    private void Start()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);

        if (TryGetComponent<PlayerAnimations>(out _playerAnimations))
            _playerAnimations.Init(_maxVelocityMagnitude);
    }

    private void Update()
    {
        IsGrounded = CharacterController.isGrounded;

        _wishedDirection = TranslateCameraDirection(GetInputVector());

        if (IsGrounded == true)
        {
            _wishedJump = _inputService.GetJumpButton(); 

            GroundMove(_wishedDirection);
            ApplyJump(_wishedJump);
        }
        else
        {
            AirMove(_wishedDirection);
        }

        if (_playerVelocity.magnitude > Mathf.Epsilon)
            MovePlayer(_playerVelocity * _speed);
    }

    private Vector3 TranslateCameraDirection(Vector3 inputVector)
    {
        if (inputVector != Vector3.zero)
            inputVector = Camera.main.transform.TransformDirection(inputVector);

        return inputVector;
    }

    private Vector3 TranslatePlayerDirection(Vector3 inputVector)
    {
        if (inputVector != Vector3.zero)
            inputVector = transform.TransformDirection(inputVector);

        return inputVector;
    }

    private void ApplyJump(bool wishedJump)
    {
        if (wishedJump == false)
        {
            _playerVelocity.y = Gravity;
            return;
        }

        _playerVelocity.y = Mathf.Sqrt(-Gravity * _jumpHeight);
    }

    private void AirMove(Vector3 wishedDirection)
    {
        if (_airControl == true)
            InertiaVelocityByInput(wishedDirection, _airControlMovement, _airAcceleration);

        _playerVelocity.y += Gravity * Time.deltaTime;
    }

    private void GroundMove(Vector3 wishedDirection) 
        => InertiaVelocityByInput(wishedDirection, _controlMovement, _groundAcceleration);

    private void InertiaVelocityByInput(Vector3 wishedDirection, float controlMovement, float accelerationByType)
    {
        if (wishedDirection == Vector3.zero)
        {
            DropVelocityAndAcceleration();

            return;
        }

        float frameAcceleration = accelerationByType * Time.deltaTime;
        if (Vector3.Angle(_accelerationVector, wishedDirection) > 90f)
            frameAcceleration *= -1f;

        _acceleration = Mathf.Clamp(_acceleration + frameAcceleration, -_maxAcceleration, _maxAcceleration);

        _accelerationVector += (wishedDirection * Mathf.Abs(frameAcceleration));
        _accelerationVector = Vector3.ClampMagnitude(_accelerationVector, _maxAcceleration);

        float prevVelocityY = _playerVelocity.y;

        _playerVelocity = Vector3.Lerp(_playerVelocity, wishedDirection * (1f + Mathf.Abs(_acceleration)),
            controlMovement * Time.deltaTime);
        _playerVelocity.y = 0f;

        _playerVelocity = Vector3.ClampMagnitude(_playerVelocity, _maxVelocityMagnitude);
        _animationVelocity = _playerVelocity;

        RotatePlayerTowardMovement();
        UpdateAnimationsIfExist(playerVelocityXZ: _animationVelocity);

        _playerVelocity.y = prevVelocityY;
    }

    private void RotatePlayerTowardMovement() 
        => transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.LookRotation(_playerVelocity), _angleControl * Time.deltaTime);

    private void DropVelocityAndAcceleration()
    {
        _acceleration = Mathf.Lerp(_acceleration, 0f, _controlDrop * Time.deltaTime);
        
        CalculateAnimationVelocity();
        UpdateAnimationsIfExist(playerVelocityXZ: _animationVelocity);

        _playerVelocity.x *= _acceleration;
        _playerVelocity.z *= _acceleration;
    }

    private void CalculateAnimationVelocity()
    {
        if (_animationVelocity == Vector3.zero)
        {
            _animationVelocity.x = _playerVelocity.x;
            _animationVelocity.z = _playerVelocity.z;
        }

        _animationVelocity.x *= Mathf.Clamp(_acceleration, _maxDropAnimationAcceleration, _acceleration);
        _animationVelocity.z *= Mathf.Clamp(_acceleration, _maxDropAnimationAcceleration, _acceleration);
    }

    private void UpdateAnimationsIfExist(Vector3 playerVelocityXZ)
    {
        if (_playerAnimations != null)
            _playerAnimations.UpdateAnimationsByVelocity(playerVelocityXZ);
    }

    private void MovePlayer(Vector3 movement) 
        => CharacterController.Move(movement * Time.deltaTime);

    private Vector3 GetInputVector()
        => _inputService.GetInputVector().normalized;
}
