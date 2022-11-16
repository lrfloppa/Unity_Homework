using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _projectilePrefab;

    private InputService _inputService;

    private const float _raycastRange = 1000f;

    private void Start()
    {
        _inputService = new InputService();
    }

    private void Update()
    {
        if (_inputService.IsShootButton())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(_inputService.MousePosition), out RaycastHit hit, _raycastRange))
        {
            GameObject projectile = GameObject.Instantiate(_projectilePrefab, _gunPoint.position, Quaternion.identity);

            projectile.transform.LookAt(hit.point);
            projectile.GetComponent<Projectile>().Direction = (hit.point - _gunPoint.position).normalized;
        }
    }
}
