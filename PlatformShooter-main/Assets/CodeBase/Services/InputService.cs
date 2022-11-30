using System;
using UnityEngine;

public class InputService : ISingletone<InputService>
{
    private static InputService _instance;

    public static InputService Instance => _instance ??= new InputService();

    private Vector3 _inputVector = Vector3.zero;
    private Vector3 _mouseMovement = Vector3.zero;

    public Vector3 MousePosition => Input.mousePosition;

    public Vector3 GetInputVector()
    {
        _inputVector.x = Input.GetAxis("Horizontal");
        _inputVector.z = Input.GetAxis("Vertical");

        return _inputVector.normalized;
    }

    public bool GetShootButton() 
        => Input.GetMouseButtonDown(0);

    public bool GetJumpButton() => Input.GetKeyDown(KeyCode.Space);

    public Vector3 GetMouseMovement()
    {
        _mouseMovement.x = Input.GetAxis("Mouse Y");
        _mouseMovement.y = Input.GetAxis("Mouse X");

        return _mouseMovement;
    }
}