using Assets.CodeBase.Infrastructure;
using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float _mouseSpeed;

    public float RotationAngleX;
    public float RotationAngleY;
    public float MaxRotationAngleX;

    public float Distance;
    public float OffsetY;

    private InputService _inputService;

    private void Start()
    {
        _inputService = AllServices.Instance.GetService<InputService>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (Target == null)
            return;

        RotateAroundTarget();
        FollowTarget();
    }

    private void RotateAroundTarget()
    {
        Vector3 mouseMovement = _inputService.GetMouseMovement();
        RotationAngleX = Mathf.Clamp(RotationAngleX - mouseMovement.x * _mouseSpeed, -MaxRotationAngleX, MaxRotationAngleX);
        RotationAngleY += mouseMovement.y * _mouseSpeed;
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(RotationAngleX, RotationAngleY, 0f);
        Vector3 position = rotation * new Vector3(0f, 0f, -CalculateDistance()) + FollowingPointPosition();

        transform.SetPositionAndRotation(position, rotation);
    }

    private float CalculateDistance()
    {
        Vector3 cameraMinPosition = Target.position + new Vector3(0f, OffsetY, 0f);
        if (Physics.Raycast(cameraMinPosition, Camera.main.transform.position - cameraMinPosition, out RaycastHit hit, Distance)){
            return (hit.point - cameraMinPosition).magnitude;
        }
        return Distance;
    }

    private Vector3 FollowingPointPosition()
    {
        Vector3 position = Target.position;
        position.y += OffsetY;

        return position;
    }

    public void SetTarget(Transform cameraTarget)
         => Target = cameraTarget;
}
