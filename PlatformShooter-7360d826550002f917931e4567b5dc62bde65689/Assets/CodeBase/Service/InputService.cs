using UnityEngine;

public class InputService
{
    private Vector3 _inputVector;

    public bool IsJumpButton() => Input.GetKeyDown(KeyCode.Space);

    public Vector3 MousePosition => Input.mousePosition;


    public Vector3 GetInputVector()
    {
        _inputVector.x = Input.GetAxis("Horizontal");
        _inputVector.y = -0.1f;
        _inputVector.z = Input.GetAxis("Vertical");

        return _inputVector;

    }

    // Input.GetAxis("Jump")

    public bool IsShootButton() 
        => Input.GetMouseButtonDown(0);
}