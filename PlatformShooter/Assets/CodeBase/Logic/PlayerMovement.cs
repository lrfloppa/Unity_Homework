using Assets.CodeBase.Infrastructure;
using System;
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

    private Vector3 _wishedDirection = Vector3.zero;
    private bool _wishedJump;

    private InputService _inputService;
    private const float Gravity = -9.81f;

    private void Start()
    {
        _inputService = AllServices.Instance.GetService<InputService>();
    }

    private void Update()
    {
        IsGrounded = CharacterController.isGrounded;

        _wishedDirection = TranslateCameraDirection(GetInputVector());

        if (CharacterController.isGrounded)
        {
            _wishedJump = _inputService.GetJumpButton(); 

            GroundMove(_wishedDirection);
            ApplyJump(_wishedJump);
        }
        else
        {
            AirMove(_wishedDirection);
        }

        
    }
    private void FixedUpdate()
    {
        MovePlayer(_playerVelocity * _speed);
    }

    private Vector3 TranslateCameraDirection(Vector3 inputVector)
    {
        if (inputVector != Vector3.zero)
            inputVector = Camera.main.transform.TransformDirection(inputVector);

        return inputVector;
    }

    private void ApplyJump(bool wishedJump)
    {
        if (wishedJump == false)
        {
            _playerVelocity.y = -0.01f;
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
            _acceleration = Mathf.Lerp(_acceleration, 0f, _controlDrop * Time.deltaTime);
            _playerVelocity.x *= _acceleration;
            _playerVelocity.z *= _acceleration;

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

        _playerVelocity.y = prevVelocityY;
    }

    private void MovePlayer(Vector3 movement)
    {
        PlayerShooting.PlayerSpeed = movement.magnitude;
        CharacterController.Move(movement);
    }

    private Vector3 GetInputVector()
        => _inputService.GetInputVector().normalized;
}
