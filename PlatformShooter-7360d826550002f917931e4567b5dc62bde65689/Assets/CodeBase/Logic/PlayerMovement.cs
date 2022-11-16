using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;

    private InputService _inputService;
    private Vector3 _verticalVelocity;
    private bool _isGrounded;

    private const float Gravity = -9.81f;

    private void Start()
    {
        _inputService = new InputService();
        _verticalVelocity = Vector3.up * Gravity;
    }

    private void Update()
    {
        Vector3 inputVector = GetInputVector();
        MovePlayer(inputVector * _speed);


        ComputeGravity();
        if (_inputService.IsJumpButton()) TryJump();
        MovePlayer(_verticalVelocity);
    }


    private void LateUpdate() 
        => _isGrounded = CharacterController.isGrounded;

    private void ComputeGravity()
    {
        if (_isGrounded == true)
            _verticalVelocity.y = -0.1f;
        else
            _verticalVelocity += Vector3.up * Gravity * Time.deltaTime;
    }

    private void TryJump()
    {
        if (_isGrounded)
        {
            _verticalVelocity = (Vector3.up * _jumpHeight);
        }
    }

    private void MovePlayer(Vector3 movement) 
        => CharacterController.Move(movement * Time.deltaTime);

    private Vector3 GetInputVector()
        => _inputService.GetInputVector();

}